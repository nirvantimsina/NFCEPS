using NFCEPS.UI.Auth;
using NFCEPS.UI.Managers;
using NFCEPS.UI.Models.ResponseModel;
using NFCEPS.UI.Pages.Auth.Managers.Interface;
using NFCEPS.UI.Pages.Auth.Managers.Route;
using NFCEPS.UI.Pages.Auth.Models.RequestModel;
using NFCEPS.UI.Pages.Auth.Models.ResponseModel;

namespace NFCEPS.UI.Pages.Auth.Managers.Implementation;

public class AuthManager(IHttpClientFactory factory, AuthSessionManager sessionManager)
    : BaseManager(sessionManager), IAuthManager
{
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var http = factory.CreateClient("API");
        var response = await http.PostAsJsonAsync(AuthRoute.Login, request);
        return await HandleResponse<LoginResponse>(response);
    }

    public async Task<ApiResponse> SignUpAsync(SignUpRequest request)
    {
        var http = factory.CreateClient("API");

        var response = await http.PostAsJsonAsync(AuthRoute.SignUp, request);
        return await HandleResponse(response);
    }
}


