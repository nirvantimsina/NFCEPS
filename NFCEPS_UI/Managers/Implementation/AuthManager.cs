using NFCEPS_UI.Endpoints;
using NFCEPS_UI.Models.RequestModel;
using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Managers;

public class AuthManager : IAuthManager
{
    private readonly HttpClient _http;

    public AuthManager(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _http.PostAsJsonAsync(AuthEndpoints.Login, loginRequest);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

        return result ?? new ApiResponse<LoginResponse>
        {
            Success = false,
            Message = "No response from server"
        };
    }
}