using NFCEPS.Shared.Models;
using NFCEPS.Shared.Wrappers;
using NFCEPS.UI.Components.Dropdown.Managers.Interface;
using NFCEPS.UI.Components.Dropdown.Managers.Route;
using NFCEPS.UI.Components.Dropdown.Models.RequestModel;
using NFCEPS.UI.Features.Auth;
using NFCEPS.UI.Shared.Infrastructure;

namespace NFCEPS.UI.Components.Dropdown.Managers.Implementation;

public class FlagDropdownManager(IHttpClientFactory factory, AuthSessionManager sessionManager)
: BaseManager(sessionManager), IFlagDropdownManager
{
    public async Task<ApiResponse<List<DropdownListModel>>> DropdownListAsync(DropdownRequestModel request)
    {
        var http = factory.CreateClient("API");
        await SetAuthHeaderAsync(http);

        string targetUrl = $"{FlagDropdownRoute.DropdownData}{request.Flag}";

        try
        {
            var response = await http.GetFromJsonAsync<ApiResponse<List<DropdownListModel>>>(targetUrl);
            return response ?? ApiResponse<List<DropdownListModel>>.Fail("No response received from the server.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<DropdownListModel>>.Fail($"Network error: {ex.Message}");
        }
    }
}
