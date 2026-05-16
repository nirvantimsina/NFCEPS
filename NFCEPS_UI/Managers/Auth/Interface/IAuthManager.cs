using NFCEPS_UI.Models.Auth.RequestModel;
using NFCEPS_UI.Models.Auth.ResponseModel;
using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Auth.Managers;

public interface IAuthManager
{   
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse> SignUpAsync(SignUpRequest request);
}