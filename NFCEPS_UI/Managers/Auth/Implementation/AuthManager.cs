using NFCEPS_UI.Auth.Route;
using NFCEPS_UI.Managers;
using NFCEPS_UI.Models.Auth.RequestModel;
using NFCEPS_UI.Models.Auth.ResponseModel;
using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Auth.Managers;

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
