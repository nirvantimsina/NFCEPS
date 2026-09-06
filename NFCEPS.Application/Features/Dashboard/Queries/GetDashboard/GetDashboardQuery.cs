using ErrorOr;
using MediatR;
using NFCEPS.Application.Models.Dashboard.ResponseModel;

namespace NFCEPS.Application.Features.Dashboard.Queries.GetDashboard
{
    public class GetDashboardQuery : IRequest<ErrorOr<DashboardResponseModel>>
    {
        public int UserId { get; set; }
    }
}
