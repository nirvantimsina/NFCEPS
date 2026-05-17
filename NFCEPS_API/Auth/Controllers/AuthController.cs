using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.Services.Interfaces;
using NFCEPS_API.Auth.Models.Request;
using Microsoft.AspNetCore.Authorization;

namespace NFCEPS_API.Controllers
{
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ApiBaseController
    {
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Inside Login method
            logger.LogInformation("Login attempt for user: {UserName}", request.UserName);

            var result = await authService.LoginAsync(request);
            return HandleResponse(result);
        }

        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestModel request)
        {
            var result = await authService.SignUpAsync(request);
            return HandleResponse(result);
        }
    }
}
