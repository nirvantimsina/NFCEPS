using NFCEPS_UI.Models.ResponseModel;
using NFCEPS_UI.Pages.Card.Models.RequestModel;

namespace NFCEPS_UI.Pages.Card.Managers.Interface;

public interface ICardManager
{
    Task<ApiResponse> AssignCardAsync(AssignCardRequestModel request);
}
