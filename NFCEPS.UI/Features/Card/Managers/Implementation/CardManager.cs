using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Features.Auth;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Features.Card.Managers.Interface;
using NFCEPS.UI.Features.Card.Managers.Route;
using NFCEPS.UI.Features.Card.Models.RequestModel;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.UI.Features.Card.Managers.Implementation;

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





