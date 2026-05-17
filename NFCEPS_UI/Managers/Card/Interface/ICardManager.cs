using NFCEPS_UI.Models.Card.RequestModel;
using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Managers.Card.Interface;

public interface ICardManager
{
    Task <ApiResponse> AssignCardAsync(AssignCardRequestModel request); 
}
