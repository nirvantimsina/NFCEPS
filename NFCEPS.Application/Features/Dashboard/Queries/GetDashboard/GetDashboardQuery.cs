using MediatR;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.Dashboard.Queries.GetDashboard
{
    public class GetDashboardQuery : IRequest<ApiResponse>
    {
        public int UserId { get; set; }
    }
}


