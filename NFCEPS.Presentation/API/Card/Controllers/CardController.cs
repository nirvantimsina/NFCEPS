using Microsoft.AspNetCore.Mvc;
using NFCEPS.Application.Features.Card.Commands.AssignCard;
using NFCEPS.Presentation.Controllers;
using MediatR;

namespace NFCEPS.Presentation.Controllers
{
    public class CardController(IMediator mediator) : ApiBaseController
    {
        [HttpPost("AssignCard")]
        public async Task<IActionResult> AssignCard([FromBody] AssignCardCommand command)
        {
            var result = await mediator.Send(command);
            return HandleResponse(result);
        }
    }
}
