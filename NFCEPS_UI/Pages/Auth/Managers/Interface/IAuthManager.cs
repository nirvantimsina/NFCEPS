using NFCEPS_UI.Models;
using NFCEPS_UI.Models.ResponseModel;
using NFCEPS_UI.Pages.Auth.Models.RequestModel;
using NFCEPS_UI.Pages.Auth.Models.ResponseModel;

namespace NFCEPS_UI.Pages.Auth.Managers.Interface;

public interface IAuthManager
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse> SignUpAsync(SignUpRequest request);
}

