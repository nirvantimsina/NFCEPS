using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Features.Auth;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Features.Dashboard.Managers.Interface;
using NFCEPS.UI.Features.Dashboard.Managers.Route;
using NFCEPS.UI.Features.Dashboard.Models.ResponseModel;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.UI.Features.Dashboard.Managers.Implementation;

public class DashboardManager(IHttpClientFactory factory, AuthSessionManager sessionManager)
    : BaseManager(sessionManager), IDashboardManager
{
    public async Task<ApiResponse<DashboardResponseModel>> DashboardDataAsync()
    {
        var http = factory.CreateClient("API");
        await SetAuthHeaderAsync(http);

        var response = await http.GetAsync(DashboardRoute.DashboardData);
        return await HandleResponse<DashboardResponseModel>(response);
    }
}




