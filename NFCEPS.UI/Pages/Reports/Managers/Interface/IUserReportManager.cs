using NFCEPS.UI.Models.ResponseModel;
using NFCEPS.UI.Pages.Reports.Models.RequestModel;
using NFCEPS.UI.Pages.Reports.Models.ResponseModel;

namespace NFCEPS.UI.Pages.Reports.Managers.Interface
{
    public interface IUserReportManager
    {
        Task<ApiResponse<List<UserReportResponseModel>>> UserReportDataAsync(UserReportRequestModel request);
    }
}


