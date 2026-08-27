using NFCEPS.UI.Features.Auth;
using NFCEPS.UI.Shared.Infrastructure;
using System.Net.Http.Headers;

namespace NFCEPS.UI.Shared.Infrastructure;

public abstract class BaseManager(AuthSessionManager sessionManager)
{
    private readonly System.Text.Json.JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

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
                ?? new ApiResponse<T> { Status = 1, Message = "Empty response from server" };
        }
        catch
        {
            return new ApiResponse<T> { Status = 1, Message = "Server Communication Error" };
        }
    }

    protected async Task<ApiResponse> HandleResponse(HttpResponseMessage response)
    {
        try
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse>(_jsonOptions);
                return errorResult ?? new ApiResponse { Status = 1, Message = $"Server error ({response.StatusCode})" };
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse>(_jsonOptions)
                ?? new ApiResponse { Status = 0, Message = "Operation completed successfully" };
        }
        catch
        {
            return new ApiResponse { Status = 1, Message = "Server Communication Error" };
        }
    }
}



