using NFCEPS_UI.Auth;
using NFCEPS_UI.Models.ResponseModel;
using System.Net.Http.Headers;

namespace NFCEPS_UI.Managers;

public abstract class BaseManager(AuthSessionManager sessionManager)
{
    protected async Task SetAuthHeaderAsync(HttpClient http)
    {
        var token = await sessionManager.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }

    protected async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        try
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>(options)
                ?? new ApiResponse<T> { Success = false, Message = "Empty response from server" };
        }
        catch
        {
            return new ApiResponse<T> { Success = false, Message = "Server Communication Error" };
        }
    }
}
