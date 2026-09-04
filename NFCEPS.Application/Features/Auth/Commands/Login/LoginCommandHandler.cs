using MediatR;
using NFCEPS.Application.Features.Auth.Queries.GetMenuList;
using NFCEPS.Application.Helpers;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Domain.Models;
using NFCEPS.Shared.Wrappers;
using System.Data;

namespace NFCEPS.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse>
    {
        private readonly IGenericRepository _repo;
        private readonly JWTHelper _jwt;
        private readonly IMediator _mediator;

        public LoginCommandHandler(IGenericRepository repo, JWTHelper jwt, IMediator mediator)
        {
            _repo = repo;
            _jwt = jwt;
            _mediator = mediator;
        }

        public async Task<ApiResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.Password is null)
                return ApiResponse.Fail("Password cannot be empty!");

            var loginParams = new { p_flag = "B", p_username = request.UserName };

            var user = await _repo.QueryFirstOrDefaultAsync<UserLoginRow>(
                "SELECT * FROM permission.fn_auth(@p_flag, @p_username);",
                loginParams,
                commandType: CommandType.Text);

            if (user is null)
                return ApiResponse.Fail("Invalid username or password");

            if (!user.IsActive)
                return ApiResponse.Fail("Account is inActive");

            if (!PasswordHelper.VerifyPassword(request.Password, user.Password))
                return ApiResponse.Fail("Invalid username or password");

            if (user.UserName is null)
                return ApiResponse.Fail("User identity profile is corrupt!");

            var listPermissions = !string.IsNullOrWhiteSpace(user.CompressedPermissions) ?
                user.CompressedPermissions.Split(',').Select(p => p.Trim()).ToList() : new List<string>();

            var menuResponse = await _mediator.Send(new GetMenuListQuery { RoleId = user.RoleId }, cancellationToken);
            var menuList = menuResponse.Success && menuResponse.Data is IEnumerable<MenuListResponseModel> menus
                ? menus.ToList()
                : new List<MenuListResponseModel>();

            var token = _jwt.GenerateToken(user.UserId, user.UserName, user.RoleId, listPermissions, Enumerable.Empty<string>());

            return ApiResponse.Ok(new LoginResponse
            {
                Token = token,
                UserName = user.UserName ?? string.Empty,
                Name = user.Name ?? string.Empty,
                RoleName = user.RoleName ?? string.Empty,
                RoleId = user.RoleId,
                Permissions = listPermissions,
                MenuList = menuList
            });
        }
    }
}


