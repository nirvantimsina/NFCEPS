using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ErrorOr; // 💡 Added for ErrorOr mapping support
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
                // 💡 FIX: Pull from ClaimTypes.Name instead of NameIdentifier
                return User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            }
        }

        protected int CurrentRoleId
        {
            get
            {
                // 💡 FIX: Pull from ClaimTypes.Role instead of NameIdentifier
                var claimValue = User.FindFirstValue(ClaimTypes.Role);
                return int.TryParse(claimValue, out var roleid) ? roleid : 0;
            }
        }

        /// <summary>
        /// New Overload: Handles all modern handlers returning ErrorOr values
        /// </summary>
        protected IActionResult HandleResponse<T>(ErrorOr<T> result)
        {
            return result.Match<IActionResult>(
                data => Ok(ApiResponse.Ok(data)),
                errors => 
                {
                    var error = errors.First();
                    
                    // 1. Resolve localized message using your glossary setup
                    string finalMessage = ErrorCodes.GetMessage(error.Code) ?? error.Description;

                    // 2. Map ErrorOr categories to industry-standard HTTP Status codes
                    return error.Type switch
                    {
                        ErrorType.NotFound => NotFound(ApiResponse.Fail(finalMessage, error.Code)),
                        ErrorType.Conflict => Conflict(ApiResponse.Fail(finalMessage, error.Code)),
                        ErrorType.Unauthorized => Unauthorized(ApiResponse.Fail(finalMessage, error.Code)),
                        _ => BadRequest(ApiResponse.Fail(finalMessage, error.Code))
                    } ;
                }
            );
        }

        /// <summary>
        /// Legacy Overload: Kept intact for any older endpoints still returning raw ApiResponse
        /// </summary>
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
