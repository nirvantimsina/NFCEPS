using NFCEPS.UI.Features.Reports.Models.RequestModel;
using NFCEPS.UI.Features.Reports.Models.ResponseModel;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.UI.Features.Reports.Managers.Interface
{
    public interface IUserReportManager
    {
        Task<ApiResponse<List<UserReportResponseModel>>> UserReportDataAsync(UserReportRequestModel request);
    }
}





