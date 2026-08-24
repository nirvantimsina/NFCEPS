using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Features.Card.Models.RequestModel;

namespace NFCEPS.UI.Features.Card.Managers.Interface;

public interface ICardManager
{
    Task<ApiResponse> AssignCardAsync(AssignCardRequestModel request);
}





