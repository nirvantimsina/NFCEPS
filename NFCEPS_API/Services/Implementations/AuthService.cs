using NFCEPS_API.Auth;
using NFCEPS_API.Models.Request;
using NFCEPS_API.Models.Response;
using NFCEPS_API.Repository.Interfaces;
using NFCEPS_API.Services.Interfaces;

namespace NFCEPS_API.Services.Implementaions;

public class AuthService : IAuthService
{
    private readonly IGenericRepository _repo;
    private readonly JWTHelper _jwt;
    private readonly PermissionService _permissions;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IGenericRepository repo,
        JWTHelper jwt,
        PermissionService permissions,
        ILogger<AuthService> logger)
    {
        _repo = repo;
        _jwt = jwt;
        _permissions = permissions;
        _logger = logger;
    }

    public async Task<ApiResponse> LoginAsync(LoginRequest request)
    {
        //fetch user by username only
        var user = await _repo.QueryFirstOrDefaultAsync<UserLoginRow>(
            "Permission.sp_Login",
            new { request.UserName });
        
        //user not found
        if (user is null)
            return ApiResponse.Fail("Invalid username or password");
        
        //account inactive
        if (!user.IsActive)
            return ApiResponse.Fail("Account is inActive");
        
        //verify password against stored BCrypt hash
        if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
            return ApiResponse.Fail("Invaild username or password");
        
        //get permission for this role from cache
        var permission = _permissions.GetAll(user.RoleId).ToList();
        
        //generate jwt
        var token = _jwt.GenerateToken(user.UserId, user.UserName, user.RoleId);

        return ApiResponse.Ok(new LoginResponse
        {
            Token = token,
            UserName = user.UserName,
            Name = user.Name,
            RoleName = user.RoleName,
            RoleId = user.RoleId,
            Permissions = permission
        });
    }
}