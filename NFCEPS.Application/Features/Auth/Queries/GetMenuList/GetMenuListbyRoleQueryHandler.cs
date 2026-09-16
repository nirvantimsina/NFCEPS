using System.Data;
using MediatR;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.Auth.Queries.GetMenuList
{
    public class GetMenuListQueryHandler : IRequestHandler<GetMenuListByRoleQuery, ApiResponse>
    {
        private readonly IGenericRepository _repo;

        public GetMenuListQueryHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse> Handle(
            GetMenuListByRoleQuery request,
            CancellationToken cancellationToken
        )
        {
            var MenuListParams = new { p_role = request.RoleId };
            var result = await _repo.QueryAsync<MenuListResponseModel>(
                "SELECT * FROM permission.get_menulist_by_role(@p_role);",
                MenuListParams,
                commandType: CommandType.Text
            );

            return result != null
                ? ApiResponse.Ok(result)
                : ApiResponse.Fail("No roles assigned to the user!");
        }
    }
}
