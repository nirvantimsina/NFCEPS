using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Features.Auth.Models.RequestModel;
using NFCEPS.UI.Features.Auth.Models.ResponseModel;

namespace NFCEPS.UI.Features.Auth.Managers.Interface;

public interface IAuthManager
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse> SignUpAsync(SignUpRequest request);
}




