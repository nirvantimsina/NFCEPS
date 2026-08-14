using NFCEPS_API.API.Auth.Models.Request;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.API.Auth.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse> LoginAsync(LoginRequest request);
    Task<ApiResponse> SignUpAsync(SignUpRequestModel request);
    Task<ApiResponse> MenuListAsync(MenuListRequestModel request);
}