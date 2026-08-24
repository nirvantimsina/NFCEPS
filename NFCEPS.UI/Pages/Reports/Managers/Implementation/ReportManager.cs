using NFCEPS.UI.Auth;
using NFCEPS.UI.Managers;
using NFCEPS.UI.Models.ResponseModel;
using NFCEPS.UI.Pages.Reports.Managers.Interface;
using NFCEPS.UI.Pages.Reports.Managers.Route;
using NFCEPS.UI.Pages.Reports.Models.RequestModel;
using NFCEPS.UI.Pages.Reports.Models.ResponseModel;

namespace NFCEPS.UI.Pages.Reports.Managers.Implementation
{
    public class UserReportManager(IHttpClientFactory factory, AuthSessionManager sessionManager) : BaseManager(sessionManager), IUserReportManager
    {
        public async Task<ApiResponse<List<UserReportResponseModel>>> UserReportDataAsync(UserReportRequestModel request)
        {
            var http = factory.CreateClient("API");
            await SetAuthHeaderAsync(http);

            var response = await http.PostAsJsonAsync(UserReportRoute.UserReportData, request);
            return await HandleResponse<List<UserReportResponseModel>>(response);
        }
    }
}
