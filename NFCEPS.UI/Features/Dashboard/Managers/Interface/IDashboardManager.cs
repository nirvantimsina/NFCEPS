using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Features.Dashboard.Models.ResponseModel;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.UI.Features.Dashboard.Managers.Interface
{
    public interface IDashboardManager
    {
        Task<ApiResponse<DashboardResponseModel>> DashboardDataAsync();
    }
}




