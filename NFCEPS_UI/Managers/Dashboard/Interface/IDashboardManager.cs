using NFCEPS_UI.Models.Dashboard.ResponseModel;
using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Managers.Dashboard.Interface
{
    public interface IDashboardManager
    {
        Task<ApiResponse<DashboardResponseModel>> DashboardDataAsync();
    }
}