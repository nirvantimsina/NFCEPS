using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.Auth.Models.ResponseModel;
using NFCEPS_API.Services.Interfaces;
using NFCEPS_API.Auth.Models.RequestModel;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            // Inside Login method
            logger.LogInformation("Login attempt for user: {UserName}", request.UserName);

            if(string.IsNullOrWhiteSpace(request.UserName) ||
               string.IsNullOrWhiteSpace(request.Password))
               return BadRequest(ApiResponse.Fail("Username and Password are required!"));

            var result = await authService.LoginAsync(request);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
