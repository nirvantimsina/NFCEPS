using NFCEPS_API.Auth;
using NFCEPS_API.Auth.Models.Request;
using NFCEPS_API.Auth.Models.Response;
using NFCEPS_API.Wrapper;
using NFCEPS_API.Repository.Interfaces;
using NFCEPS_API.Services.Interfaces;
using System.Data.Common;
using System.Data;

namespace NFCEPS_API.Services.Implementaions;

public class AuthService(IGenericRepository repo, JWTHelper jwt) : IAuthService
{
    public async Task<ApiResponse> LoginAsync(LoginRequest request)
    {
        if (request.Password is null)
            return ApiResponse.Fail("Password cannot be empty!");

        // Use lowercase parameter names to match the Postgres function arguments
        var loginParams = new { p_flag = "B", p_username = request.UserName };

        // Explicitly pass CommandType.Text to bypass the StoredProcedure engine parser
        // This is the service for validating the user login
        var user = await repo.QueryFirstOrDefaultAsync<UserLoginRow>(
            "SELECT * FROM permission.fn_auth(@p_flag, @p_username);",
            loginParams,
            commandType: CommandType.Text);


        //user not found
        if (user is null)
            return ApiResponse.Fail("Invalid username or password");

        //account inactive
        if (!user.isactive)
            return ApiResponse.Fail("Account is inActive");

        //userName is null
        if (request.Password is null)
            return ApiResponse.Fail("Password cannot be empty!");

        //verify password against stored BCrypt hash
        if (!PasswordHelper.VerifyPassword(request.Password, user.password))
            return ApiResponse.Fail("Invalid username or password");

        //userName is null
        if (user.username is null)
            return ApiResponse.Fail("User identity profile is corrupt!");

        var listPermissions = !string.IsNullOrWhiteSpace(user.compressedpermissions) ?
        user.compressedpermissions.Split(',').Select(p => p.Trim()).ToList() : new List<string>();

        var token = jwt.GenerateToken(user.userid, user.username, user.roleid, listPermissions);

        return ApiResponse.Ok(new LoginResponse
        {
            Token = token,
            UserName = user.username ?? string.Empty,
            Name = user.name ?? string.Empty,
            RoleName = user.rolename ?? string.Empty,
            RoleId = user.roleid,
            Permissions = listPermissions
        });
    }

    public async Task<ApiResponse> SignUpAsync(SignUpRequestModel request)
    {
        try
        {
            var hashedPassword = PasswordHelper.HashPassword(request.Password);
            var signUpParams = new
            {
                p_flag = "A",
                p_username = request.UserName,
                p_name = request.Name,
                p_address = request.Address,
                p_phone = request.Phone,
                p_password = hashedPassword
            };

            await repo.ExecuteAsync(
                "SELECT permission.fn_auth(@p_flag, @p_username, @p_name, @p_address, @p_phone, @p_password)",
                signUpParams,
                commandType: CommandType.Text);

            return ApiResponse.Ok();
        }
        catch (DbException ex)
        {
            if (ex.Message.Contains("violates unique constraint", StringComparison.OrdinalIgnoreCase) || ex.SqlState == "23505")
            {
                return ApiResponse.Fail("Username or Phone Number already exists.");
            }

            return ApiResponse.Fail("A database error occurred during registration!");
        }
        catch
        {
            return ApiResponse.Fail("An unexpected error occurred!");
        }
    }
}