using NFCEPS_API.API.Card.Models.Request;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.API.Card.Services.Interface
{
    public interface ICardService
    {
        Task<ApiResponse> AssignCardAsync(AssignCardRequestModel request);
    }
}