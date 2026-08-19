using NFCEPS_UI.Models.ResponseModel;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Pages.Dashboard.Managers.Interface;
using NFCEPS_UI.Pages.Dashboard.Managers.Route;
using NFCEPS_UI.Managers;
using NFCEPS_UI.Pages.Dashboard.Models.ResponseModel;

namespace NFCEPS_UI.Pages.Dashboard.Managers.Implementation;

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

