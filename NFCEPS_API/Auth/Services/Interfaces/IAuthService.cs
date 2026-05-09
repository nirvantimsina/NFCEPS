using NFCEPS_API.Auth.Models.RequestModel;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse> LoginAsync(LoginRequest request);
}