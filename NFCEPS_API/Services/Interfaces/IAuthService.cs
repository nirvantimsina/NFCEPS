using NFCEPS_API.Models.Request;
using NFCEPS_API.Models.Response;

namespace NFCEPS_API.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse> LoginAsync(LoginRequest request);
}