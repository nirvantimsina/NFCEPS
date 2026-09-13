using NFCEPS.UI.Features.Auth.Models.RequestModel;
using NFCEPS.UI.Features.Auth.Models.ResponseModel;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.UI.Features.Auth.Managers.Interface;

public interface IAuthManager
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse> SignUpAsync(SignUpRequest request);
}




