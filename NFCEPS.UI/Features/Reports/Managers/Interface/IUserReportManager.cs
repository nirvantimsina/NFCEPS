using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Features.Reports.Models.RequestModel;
using NFCEPS.UI.Features.Reports.Models.ResponseModel;

namespace NFCEPS.UI.Features.Reports.Managers.Interface
{
    public interface IUserReportManager
    {
        Task<ApiResponse<List<UserReportResponseModel>>> UserReportDataAsync(UserReportRequestModel request);
    }
}





