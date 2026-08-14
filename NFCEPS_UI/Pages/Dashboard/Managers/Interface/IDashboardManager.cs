using NFCEPS_UI.Models.ResponseModel;
using NFCEPS_UI.Pages.Dashboard.Models.ResponseModel;

namespace NFCEPS_UI.Pages.Dashboard.Managers.Interface
{
    public interface IDashboardManager
    {
        Task<ApiResponse<DashboardResponseModel>> DashboardDataAsync();
    }
}