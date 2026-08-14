using Microsoft.AspNetCore.Mvc;
using NFCEPS_API.API.Reports.Models.RequestModel;
using NFCEPS_API.API.Reports.Services.Interface;
using NFCEPS_API.Controllers;

namespace NFCEPS_API.API.Reports.Controllers
{
    [ApiController]
    public class UserReportController(IUserReportService userReportService) : ApiBaseController
    {
        [HttpGet("UserReport")]
        public async Task<IActionResult> GetUserReportData([FromQuery] UserReportRequestModel request)
        {
            request.UserId = request.UserId;
            var result = await userReportService.GetUsersReportAsync(request);

            return HandleResponse(result);
        }
    }
}
