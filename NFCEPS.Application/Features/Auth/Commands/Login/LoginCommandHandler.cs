using ErrorOr; // 💡 Switch from ApiResponse to ErrorOr
using MediatR;
using NFCEPS.Application.Features.MenuSetup.Queries.GetMenuList; // 💡 Updated to match your correct folder namespace
using NFCEPS.Application.Helpers;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Domain.Models;
using NFCEPS.Shared.Wrappers;
using System.Data;

namespace NFCEPS.Application.Features.Auth.Commands.Login
{
    // 💡 Changed return type parameter from ApiResponse to ErrorOr<LoginResponse>
    public class LoginCommandHandler(IGenericRepository repo, JWTHelper jwt, IMediator mediator) 
        : IRequestHandler<LoginCommand, ErrorOr<LoginResponse>>
    {
        public async Task<ErrorOr<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                return Error.Validation(ErrorCodes.MissingRequiredField!, "Password cannot be empty!");

            var loginParams = new { p_flag = "B", p_username = request.UserName };

            var user = await repo.QueryFirstOrDefaultAsync<UserLoginRow>(
                "SELECT * FROM permission.fn_auth(@p_flag, @p_username);",
                loginParams,
                commandType: CommandType.Text);

            // 💡 Return type-safe Domain Errors mapped to your centralized ErrorCodes definitions
            if (user is null)
                return Error.Validation(ErrorCodes.InvalidCredentials!, "Invalid username or password");

            if (!user.IsActive)
                return Error.Validation(ErrorCodes.AccountInactive!, "Account is inactive");

            if (!PasswordHelper.VerifyPassword(request.Password, user.Password!))
                return Error.Validation(ErrorCodes.InvalidCredentials!, "Invalid username or password");

            if (user.UserName is null)
                return Error.Unexpected(ErrorCodes.GeneralError!, "User identity profile is corrupt!");

            var listPermissions = !string.IsNullOrWhiteSpace(user.CompressedPermissions) ?
                user.CompressedPermissions.Split(',').Select(p => p.Trim()).ToList() : [];

            // 💡 FIX COMPILATION: Safely resolve your updated ErrorOr menu response flow
            var menuResponse = await mediator.Send(new GetMenuListQuery { RoleId = user.RoleId }, cancellationToken);
            
            var menuList = !menuResponse.IsError 
                ? menuResponse.Value 
                : []; // Fallback to an empty list if no menus are assigned or an error occurs

            var token = jwt.GenerateToken(user.UserId, user.UserName, user.RoleId, listPermissions, Enumerable.Empty<string>());

            // 💡 Return raw data. The controller's .Match() method handles wrapping it into ApiResponse.Ok()
            return new LoginResponse
            {
                Token = token,
                UserName = user.UserName ?? string.Empty,
                Name = user.Name ?? string.Empty,
                RoleName = user.RoleName ?? string.Empty,
                RoleId = user.RoleId,
                Permissions = listPermissions,
                MenuList = menuList
            };
        }
    }
}
