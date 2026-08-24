using NFCEPS.UI.Models.ResponseModel;
using NFCEPS.UI.Pages.Card.Models.RequestModel;

namespace NFCEPS.UI.Pages.Card.Managers.Interface;

public interface ICardManager
{
    Task<ApiResponse> AssignCardAsync(AssignCardRequestModel request);
}


