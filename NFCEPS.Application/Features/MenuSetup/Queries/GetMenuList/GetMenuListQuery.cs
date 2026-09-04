using MediatR;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.MenuSetup.Queries.GetMenuList
{
    public class GetMenuListQuery : IRequest<ApiResponse>
    {
        public int RoleId { get; set; }
    }
}