using NFCEPS_UI.Auth;
using NFCEPS_UI.Managers.Card.Interface;
using NFCEPS_UI.Managers.Card.Route;
using NFCEPS_UI.Models.Card.RequestModel;
using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Managers.Card.Implementation;

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
