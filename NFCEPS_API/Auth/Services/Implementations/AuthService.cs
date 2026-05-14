using NFCEPS_API.Auth;
using NFCEPS_API.Auth.Models.RequestModel;
using NFCEPS_API.Auth.Models.ResponseModel;
using NFCEPS_API.Wrapper;
using NFCEPS_API.Repository.Interfaces;
using NFCEPS_API.Services.Interfaces;

namespace NFCEPS_API.Services.Implementaions;

public class AuthService(IGenericRepository repo,
                        JWTHelper jwt) : IAuthService
{
    public async Task<ApiResponse> LoginAsync(LoginRequest request)
    {
        //fetch user by username only
        var user = await repo.QueryFirstOrDefaultAsync<UserLoginRow>(
            "Permission.sp_Login",
            new { request.UserName });

        //user not found
        if (user is null)
            return ApiResponse.Fail("Invalid username or password");

        //account inactive
        if (!user.IsActive)
            return ApiResponse.Fail("Account is inActive");

        //userName is null
        if (request.Password is null)
            return ApiResponse.Fail("Password cannot be empty!");

        //verify password against stored BCrypt hash
        if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
            return ApiResponse.Fail("Invaild username or password");

        //userName is null
        if (user.UserName is null)
            return ApiResponse.Fail("User identity profile is corrupt!");

        var listPermissions = !string.IsNullOrWhiteSpace(user.CompressedPermissions) ? 
        user.CompressedPermissions.Split(',').Select(p => p.Trim()).ToList() : new List<string>();

        var token = jwt.GenerateToken(user.UserId, user.UserName, user.RoleId, listPermissions);

        return ApiResponse.Ok(new LoginResponse
        {
            Token = token,
            UserName = user.UserName ?? string.Empty,
            Name = user.Name ?? string.Empty,
            RoleName = user.RoleName ?? string.Empty,
            RoleId = user.RoleId,
            Permissions = listPermissions
        });
    }
}