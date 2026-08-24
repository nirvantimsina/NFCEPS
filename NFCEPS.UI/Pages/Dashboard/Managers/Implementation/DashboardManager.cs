using NFCEPS.UI.Auth;
using NFCEPS.UI.Managers;
using NFCEPS.UI.Models.ResponseModel;
using NFCEPS.UI.Pages.Dashboard.Managers.Interface;
using NFCEPS.UI.Pages.Dashboard.Managers.Route;
using NFCEPS.UI.Pages.Dashboard.Models.ResponseModel;

namespace NFCEPS.UI.Pages.Dashboard.Managers.Implementation;

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

