using NFCEPS_UI.Auth;
using NFCEPS_UI.Models.ResponseModel;
using System.Net.Http.Headers;

namespace NFCEPS_UI.Managers;

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
                ?? new ApiResponse<T> { Success = false, Message = "Empty response from server" };
        }
        catch
        {
            return new ApiResponse<T> { Success = false, Message = "Server Communication Error" };
        }
    }

        protected async Task<ApiResponse> HandleResponse(HttpResponseMessage response)
    {
        try
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse>(_jsonOptions);
                return errorResult ?? new ApiResponse { Success = false, Message = $"Server error ({response.StatusCode})" };
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse>(_jsonOptions)
                ?? new ApiResponse { Success = true, Message = "Operation completed successfully" };
        }
        catch
        {
            return new ApiResponse { Success = false, Message = "Server Communication Error" };
        }
    }
}


