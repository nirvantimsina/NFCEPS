using NFCEPS.UI.Auth;
using NFCEPS.UI.Managers;
using NFCEPS.UI.Models.ResponseModel;
using NFCEPS.UI.Pages.Card.Managers.Interface;
using NFCEPS.UI.Pages.Card.Managers.Route;
using NFCEPS.UI.Pages.Card.Models.RequestModel;

namespace NFCEPS.UI.Pages.Card.Managers.Implementation;

public class CardManager(
    IHttpClientFactory factory,
    AuthSessionManager sessionManager) : BaseManager(sessionManager), ICardManager
{
    public async Task<ApiResponse> AssignCardAsync(AssignCardRequestModel request)
    {
        var http = factory.CreateClient("API");
        await SetAuthHeaderAsync(http);

        var response = await http.PostAsJsonAsync(CardRoute.AssignCard, request);
        return await HandleResponse(response);
    }
}


