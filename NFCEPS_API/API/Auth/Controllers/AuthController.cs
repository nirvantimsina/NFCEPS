using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NFCEPS_API.Controllers;
using NFCEPS_API.API.Auth.Models.Request;
using NFCEPS_API.API.Auth.Services.Interfaces;
using NFCEPS_API.Wrapper;

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

        [HttpGet("MenuList")]
        [Authorize]
        public async Task<IActionResult> MenuList()
        {
            if (CurrentRoleId == 0)
            {
                return Unauthorized(ApiResponse.Fail("Invalid or missing Role ID in token."));
            }

            var result = await authService.MenuListAsync(CurrentRoleId);
            return HandleResponse(result);
        }
    }
}
