using MediatR;
using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Features.Reports.Queries.GetUserReport
{
    public class GetUserReportQuery : IRequest<ApiResponse>
    {
        public int UserId { get; set; }
    }
}


