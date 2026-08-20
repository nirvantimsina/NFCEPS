using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFCEPS.Application.Features.Auth.Commands.Login;
using NFCEPS.Application.Features.Auth.Commands.SignUp;
using NFCEPS.Application.Features.Auth.Queries.GetMenuList;
using NFCEPS.Domain.Models;

namespace NFCEPS.Presentation.Controllers
{
    [ApiController]
    public class AuthController(IMediator mediator, ILogger<AuthController> logger) : ApiBaseController
    {
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            logger.LogInformation("Login attempt for user: {UserName}", command.UserName);
            var result = await mediator.Send(command);
            return HandleResponse(result);
        }

        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var result = await mediator.Send(command);
            return HandleResponse(result);
        }

        [HttpGet("MenuList")]
        [Authorize]
        public async Task<IActionResult> MenuList()
        {
            if (CurrentRoleId == 0)
                return Unauthorized(ApiResponse.Fail("Invalid or missing Role ID in token."));

            var result = await mediator.Send(new GetMenuListQuery { RoleId = CurrentRoleId });
            return HandleResponse(result);
        }
    }
}

