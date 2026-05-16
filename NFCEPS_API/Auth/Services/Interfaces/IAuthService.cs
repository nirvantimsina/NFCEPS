using NFCEPS_API.Auth.Models.Request;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse> LoginAsync(LoginRequest request);
    Task<ApiResponse> SignUpAsync(SignUpRequestModel request);
}