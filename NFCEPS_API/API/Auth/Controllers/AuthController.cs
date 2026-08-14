using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NFCEPS_API.Controllers;
using NFCEPS_API.API.Auth.Models.Request;
using NFCEPS_API.API.Auth.Services.Interfaces;

namespace NFCEPS_API.API.Auth.Controllers
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

        [HttpPost("MenuList")]
        public async Task<IActionResult> MenuList([FromQuery] MenuListRequestModel request)
        {
            var result = await authService.MenuListAsync(request);
            return HandleResponse(result);
        }
    }
}
