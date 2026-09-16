using NFCEPS.Shared.Wrappers;
using NFCEPS.UI.Features.Card.Models.RequestModel;

namespace NFCEPS.UI.Features.Card.Managers.Interface;

public interface ICardManager
{
    Task<ApiResponse> AssignCardAsync(AssignCardRequestModel request);
}
