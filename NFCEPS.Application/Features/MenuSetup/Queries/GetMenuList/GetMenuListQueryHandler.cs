using System.Data;
using MediatR;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.MenuSetup.Queries.GetMenuList
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
            var paramsObj = new { p_roleid = request.RoleId };
            var result = await _repo.QueryAsync<MenuListResponseModel>(
                "select * from permission.get_assigned_menulist(@p_roleid);",
                paramsObj,
                commandType: CommandType.Text);
            
            return result != null ? ApiResponse.Ok(result) : ApiResponse.Fail("Data not found!");
        }
    }
}