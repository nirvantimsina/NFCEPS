using NFCEPS.Shared.Models;
using NFCEPS.Shared.Wrappers;

public class CommonDataService
{
    private readonly HttpClient _http;

    public CommonDataService(HttpClient http) => _http = http;

    public async Task<List<DropdownListModel>> GetDropdownOptionsAsync(string flag)
    {
        var response = await _http.GetFromJsonAsync<ApiResponse<List<DropdownListModel>>>(
            $"api/common/dropdowns?flag={flag}"
        );

        if (response is { Success: true, Data: not null })
        {
            return response.Data;
        }

        return new List<DropdownListModel>();
    }
}
