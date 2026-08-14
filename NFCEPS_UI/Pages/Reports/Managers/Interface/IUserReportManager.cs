using NFCEPS_UI.Models.ResponseModel;
using NFCEPS_UI.Pages.Reports.Models.ResponseModel;

namespace NFCEPS_UI.Pages.Reports.Managers.Interface
{
    public interface IUserReportManager
    {
        Task<ApiResponse<UserReportResponseModel>> UserReportDataAsync();
    }
}
