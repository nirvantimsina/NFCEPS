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
        protected IActionResult HandleResponse(ApiResponse result)
        {
            if (result == null)
                return StatusCode(500, ApiResponse.Fail("An unexpected error occured!"));

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}