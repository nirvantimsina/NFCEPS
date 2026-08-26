using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFCEPS.Presentation.Controllers;
using NFCEPS.Application.Features.Hardware.DeductBalance.Commands;

namespace NFCEPS.Presentation.Controller
{
    public class DeductBalanceController(IMediator mediator) : ApiBaseController
    {
        [HttpPost("DeductBalance")]
        public async Task<IActionResult> DeductBalance([FromBody] DeductBalanceCommand command)
        {
            var result = await mediator.Send(command);
            return HandleResponse(result);
        }
    }
}
