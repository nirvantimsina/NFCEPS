using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFCEPS.Application.Features.Card.Commands.AssignCard;
using NFCEPS.Shared.Wrappers;

namespace NFCEPS.Presentation.Controllers
{
    public class CardController(IMediator mediator) : ApiBaseController
    {
        [HttpPost("AssignCard")]
        public async Task<IActionResult> AssignCard([FromBody] AssignCardCommand command)
        {
            ErrorOr<AssignCardResponseModel> result = await mediator.Send(command);

            return result.Match<IActionResult>(data => Ok(ApiResponse.Ok(data, "Card assigned successfully!")),
                errors => BadRequest(ApiResponse.Fail(errors.First().Description, errors.First().Code)));
        }
    }
}
