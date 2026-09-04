using MediatR;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.Dashboard.Queries.GetDashboard
{
    public class GetDashboardQuery : IRequest<ApiResponse>
    {
        public int UserId { get; set; }
    }
}
