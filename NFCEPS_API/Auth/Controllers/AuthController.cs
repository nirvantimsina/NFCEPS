using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.Services.Interfaces;
using NFCEPS_API.Auth.Models.RequestModel;
using NFCEPS_API.Wrapper;
using Microsoft.AspNetCore.Authorization;

namespace NFCEPS_API.Controllers
{

    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ApiBaseController
    {
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Inside Login method
            logger.LogInformation("Login attempt for user: {UserName}", request.UserName);

            if(string.IsNullOrWhiteSpace(request.UserName) ||
               string.IsNullOrWhiteSpace(request.Password))
               return BadRequest(ApiResponse.Fail("Username and Password are required!"));

            var result = await authService.LoginAsync(request);

            return HandleResponse(result);
        }
    }
}
