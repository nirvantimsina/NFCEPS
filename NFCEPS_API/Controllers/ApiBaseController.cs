using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        protected int CurrentUserId
        {
            get
            {
                var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(claimValue, out var id) ? id : 0;
            }
        }

        protected string? CurrentUserName
        {
            get
            {
                var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return claimValue ?? string.Empty;
            }
        }
        protected IActionResult HandleResponse(ApiResponse result)
        {
            if (result == null)
                return StatusCode(500, ApiResponse.Fail("Unexpected error", 500));

            var statusCode = result.StatusCode;

            if (statusCode == 0)
            {
                statusCode = result.Success ? 200 : 400;
            }

            return StatusCode(statusCode, result);
        }
    }
}