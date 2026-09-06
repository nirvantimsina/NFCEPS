using System.Data;
using ErrorOr;
using MediatR;
using NFCEPS.Application.Common.Extensions;
using NFCEPS.Application.Interfaces;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.MenuSetup.Queries.GetMenuList
{
    public class GetMenuListQueryHandler : IRequestHandler<GetMenuListQuery, ErrorOr<List<MenuListResponseModel>>>
    {
        private readonly IGenericRepository _repo;

        public GetMenuListQueryHandler(IGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<ErrorOr<List<MenuListResponseModel>>> Handle(GetMenuListQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.QueryAsync<MenuListResponseModel>(
                "select * from permission.get_assigned_menulist(@p_roleid);",
                new { p_roleid = request.RoleId },
                commandType: CommandType.Text);

            return result.ToDbResultList();
        }
    }
}