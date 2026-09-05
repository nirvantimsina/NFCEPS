using ErrorOr;
using MediatR;
using NFCEPS.Application.Models.Auth.Response;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.MenuSetup.Queries.GetMenuList
{
    public class GetMenuListQuery : IRequest<ErrorOr<List<MenuListResponseModel>>>
    {
        public int RoleId { get; set; }
    }
}