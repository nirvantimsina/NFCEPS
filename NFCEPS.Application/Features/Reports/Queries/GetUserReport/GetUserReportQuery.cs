using MediatR;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Application.Features.Reports.Queries.GetUserReport
{
    public class GetUserReportQuery : IRequest<ApiResponse>
    {
        public int UserId { get; set; }
    }
}


