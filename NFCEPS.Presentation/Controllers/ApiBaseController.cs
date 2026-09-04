using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFCEPS.Shared.Wrappers;
using System.Security.Claims;

namespace NFCEPS.Presentation.Controllers
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
        protected int CurrentRoleId
        {
            get
            {
                var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(claimValue, out var roleid) ? roleid : 0;
            }
        }
        protected IActionResult HandleResponse(ApiResponse result)
        {
            if (result == null)
            {
                return StatusCode(500, ApiResponse.Fail("An unexpected error occurred!"));
            }

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}




