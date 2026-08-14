using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.API.Dashboard.Models.RequestModel;
using NFCEPS_API.API.Dashboard.Services.Interface;
using NFCEPS_API.Controllers;

namespace NFCEPS_API.API.Dashboard.Controllers
{
    [ApiController]
    public class DashboardController(IDashboardService dashboardService) : ApiBaseController
    {
        [HttpGet("DashboardData")]
        public async Task<IActionResult> GetDashboardData([FromQuery] DashboardRequestModel request)
        {
            request.UserId = CurrentUserId;
            var result = await dashboardService.GetDashboardDataAsync(request);

            return HandleResponse(result);
        }
    }
}
