using NFCEPS_UI.Models.RequestModel;
using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Managers;

public interface IAuthManager
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest loginRequest);
}