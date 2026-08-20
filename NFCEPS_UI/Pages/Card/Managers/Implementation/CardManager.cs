using NFCEPS_UI.Auth;
using NFCEPS_UI.Managers;
using NFCEPS_UI.Models.ResponseModel;
using NFCEPS_UI.Pages.Card.Managers.Interface;
using NFCEPS_UI.Pages.Card.Managers.Route;
using NFCEPS_UI.Pages.Card.Models.RequestModel;

namespace NFCEPS_UI.Pages.Card.Managers.Implementation;

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


