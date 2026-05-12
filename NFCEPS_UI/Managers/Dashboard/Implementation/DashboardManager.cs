using NFCEPS_UI.Managers.Dashboard.Route;
using NFCEPS_UI.Models.Dashboard.ResponseModel;
using NFCEPS_UI.Models.ResponseModel;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Managers.Dashboard.Interface;

namespace NFCEPS_UI.Managers.Dashboard.Implementation;

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