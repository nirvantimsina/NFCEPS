using MediatR;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Shared.Wrappers;
using System.Data;

namespace NFCEPS.Application.Features.Auth.Queries.GetMenuList
{
    public class GetMenuListQueryHandler : IRequestHandler<GetMenuListQuery, ApiResponse>
    {
        private readonly IGenericRepository _repo;

        public GetMenuListQueryHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse> Handle(GetMenuListQuery request, CancellationToken cancellationToken)
        {
            var MenuListParams = new { p_flag = "A", p_role = request.RoleId };
            var result = await _repo.QueryAsync<MenuListResponseModel>(
                "SELECT * FROM permission.fn_MenuList(@p_flag, @p_role);",
                MenuListParams,
                commandType: CommandType.Text);

            return result != null ? ApiResponse.Ok(result) : ApiResponse.Fail("No roles assigned to the user!");
        }
    }
}


