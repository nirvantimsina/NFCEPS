using MediatR;
using NFCEPS.Domain.Models;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.Auth.Queries.GetMenuList
{
    public class GetMenuListByRoleQuery : IRequest<ApiResponse>
    {
        public int RoleId { get; set; }
    }
}


