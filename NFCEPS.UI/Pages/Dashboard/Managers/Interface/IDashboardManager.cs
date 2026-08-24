using NFCEPS.UI.Models.ResponseModel;
using NFCEPS.UI.Pages.Dashboard.Models.ResponseModel;

namespace NFCEPS.UI.Pages.Dashboard.Managers.Interface
{
    public interface IDashboardManager
    {
        Task<ApiResponse<DashboardResponseModel>> DashboardDataAsync();
    }
}

