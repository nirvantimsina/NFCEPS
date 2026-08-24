using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Features.Auth;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Features.Reports.Managers.Interface;
using NFCEPS.UI.Features.Reports.Managers.Route;
using NFCEPS.UI.Features.Reports.Models.RequestModel;
using NFCEPS.UI.Features.Reports.Models.ResponseModel;

namespace NFCEPS.UI.Features.Reports.Managers.Implementation
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



