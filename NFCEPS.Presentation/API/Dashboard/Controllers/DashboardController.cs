using Microsoft.AspNetCore.Mvc;
using NFCEPS.Application.Features.Dashboard.Queries.GetDashboard;
using NFCEPS.Presentation.Controllers;
using MediatR;

namespace NFCEPS.Presentation.Controllers
{
    [ApiController]
    public class DashboardController(IMediator mediator) : ApiBaseController
    {
        [HttpGet("DashboardData")]
        public async Task<IActionResult> GetDashboardData([FromQuery] GetDashboardQuery query)
        {
            query.UserId = CurrentUserId;
            var result = await mediator.Send(query);
            return HandleResponse(result);
        }
    }
}
