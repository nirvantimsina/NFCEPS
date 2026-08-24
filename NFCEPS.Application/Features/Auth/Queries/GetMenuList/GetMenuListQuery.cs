using MediatR;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.Auth.Queries.GetMenuList
{
    public class GetMenuListQuery : IRequest<ApiResponse>
    {
        public int RoleId { get; set; }
    }
}


