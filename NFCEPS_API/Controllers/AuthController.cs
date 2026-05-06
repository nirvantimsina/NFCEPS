using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.Models.Response;
using NFCEPS_API.Services.Interfaces;
using NFCEPS_API.Models.Request;
using NFCEPS_API.Services.Implementaions;

namespace NFCEPS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.UserName) ||
               string.IsNullOrWhiteSpace(request.Password))
               return BadRequest(ApiResponse.Fail("Username and Password are required!"));

            var result = await _authService.LoginAsync(request);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);

            // Inside Login method
            _logger.LogInformation("Login attempt for user: {UserName}", request.UserName);

        }
    }
}
