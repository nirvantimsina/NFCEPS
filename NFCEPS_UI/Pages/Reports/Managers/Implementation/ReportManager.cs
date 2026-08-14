using NFCEPS_UI.Auth;
using NFCEPS_UI.Managers;
using NFCEPS_UI.Models.ResponseModel;
using NFCEPS_UI.Pages.Reports.Managers.Interface;
using NFCEPS_UI.Pages.Reports.Managers.Route;
using NFCEPS_UI.Pages.Reports.Models.ResponseModel;

namespace NFCEPS_UI.Pages.Reports.Managers.Implementation
{
    public class ReportManager(IHttpClientFactory factory, AuthSessionManager sessionManager) : BaseManager(sessionManager), IUserReportManager
    {
        public async Task<ApiResponse<UserReportResponseModel>> UserReportDataAsync()
        {
            var http = factory.CreateClient("API");
            await SetAuthHeaderAsync(http);

            var response = await http.GetAsync(UserReportRoute.UserReportData);
            return await HandleResponse<UserReportResponseModel>(response);
        }
    }
}
