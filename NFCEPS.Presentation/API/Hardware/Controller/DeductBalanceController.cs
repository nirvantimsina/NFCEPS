using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFCEPS.Presentation.Controllers;
using NFCEPS.Application.Features.Hardware.DeductBalance.Commands;
using ErrorOr;
using NFCEPS.Domain.Models;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Presentation.Controller
{
    public class DeductBalanceController(IMediator mediator) : ApiBaseController
    {
        [HttpPost("DeductBalance")]
        public async Task<IActionResult> DeductBalance([FromBody] DeductBalanceCommand command)
        {
            ErrorOr<StatusResponse> result = await mediator.Send(command);
            
            return result.Match<IActionResult>(data => Ok(ApiResponse.Ok(data)),
                errors => BadRequest(ApiResponse.Fail(errors.First().Description, errors.First().Code)));
        }
    }
}
