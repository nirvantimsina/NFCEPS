using System.Data;
using NFCEPS_API.Dashboard.Models.RequestModel;
using NFCEPS_API.Dashboard.Models.ResponseModel;
using NFCEPS_API.Dashboard.Services.Interface;
using NFCEPS_API.Repository.Interfaces;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.Dashboard.Services.Implementation;

public class DashboardService(
    IGenericRepository repo) : IDashboardService
{
    public async Task<ApiResponse> GetDashboardDataAsync(DashboardRequestModel request)
    {
        var paramsObj = new
        {
            p_flag = "A",
            p_userid = request.UserId
        };

        var result = await repo.QueryFirstOrDefaultAsync<DashboardResponseModel>(
            "SELECT * FROM public.fn_dashboard(@p_flag, @p_userid);",
            paramsObj,
            commandType: CommandType.Text);

        return result != null
            ? ApiResponse.Ok(result)
            : ApiResponse.Fail("Dashboard data not found");
    }
}
