using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFCEPS.Application.Features.Common.Queries.GetDropdownItems;

namespace NFCEPS.Presentation.Controllers;

public class CommonController : ApiBaseController
{
    private readonly IMediator _mediator;

    // Inject IMediator via constructor
    public CommonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dropdowns")]
    public async Task<IActionResult> GetDropdowns([FromQuery] GetDropdownItemsQuery query)
    {
        var result = await _mediator.Send(query);
        return HandleResponse(result);
    }
}
