using MediatR;
using Microsoft.AspNetCore.Mvc;
using NFCEPS.Application.Features.Reports.Queries.GetUserReport;

namespace NFCEPS.Presentation.Controllers
{
    [ApiController]
    public class UserReportController(IMediator mediator) : ApiBaseController
    {
        [HttpPost("UserReportData")]
        public async Task<IActionResult> GetUserReportData([FromBody] GetUserReportQuery query)
        {
            var result = await mediator.Send(query);
            return HandleResponse(result);
        }
    }
}
