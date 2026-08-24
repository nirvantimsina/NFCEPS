using NFCEPS.UI.Models.ResponseModel;
using NFCEPS.UI.Pages.Auth.Models.RequestModel;
using NFCEPS.UI.Pages.Auth.Models.ResponseModel;

namespace NFCEPS.UI.Pages.Auth.Managers.Interface;

public interface IAuthManager
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse> SignUpAsync(SignUpRequest request);
}

