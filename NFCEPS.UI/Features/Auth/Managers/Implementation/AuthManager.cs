using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Features.Auth;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Features.Auth.Managers.Interface;
using NFCEPS.UI.Features.Auth.Managers.Route;
using NFCEPS.UI.Features.Auth.Models.RequestModel;
using NFCEPS.UI.Features.Auth.Models.ResponseModel;

namespace NFCEPS.UI.Features.Auth.Managers.Implementation;

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





