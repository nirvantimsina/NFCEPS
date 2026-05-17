using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.Controllers;
using NFCEPS_API.Dashboard.Models.RequestModel;
using NFCEPS_API.Dashboard.Services.Interface;

namespace NFCEPS_API.Dashboard.Controllers
{
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
