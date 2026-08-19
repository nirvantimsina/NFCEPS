using Microsoft.AspNetCore.Mvc;
using NFCEPS.Application.Features.Reports.Queries.GetUserReport;
using NFCEPS.Presentation.Controllers;
using MediatR;

namespace NFCEPS.Presentation.Controllers
{
    [ApiController]
    public class UserReportController(IMediator mediator) : ApiBaseController
    {
        [HttpGet("UserReport")]
        public async Task<IActionResult> GetUserReportData([FromQuery] GetUserReportQuery query)
        {
            var result = await mediator.Send(query);
            return HandleResponse(result);
        }
    }
}
