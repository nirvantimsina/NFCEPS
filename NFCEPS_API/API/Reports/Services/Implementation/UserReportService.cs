using NFCEPS_API.API.Reports.Models.RequestModel;
using NFCEPS_API.API.Reports.Models.ResponseModel;
using NFCEPS_API.API.Reports.Services.Interface;
using NFCEPS_API.Repository.Interfaces;
using NFCEPS_API.Wrapper;
using System.Data;

namespace NFCEPS_API.API.Reports.Services.Implementation
{
    public class UserReportService(IGenericRepository repo) : IUserReportService
    {
        public async Task<ApiResponse> GetUsersReportAsync(UserReportRequestModel request)
        {
            var paramobj = new
            {
                p_flag = "A",
                p_userid = request.UserId
            };

            var result = await repo.QueryFirstOrDefaultAsync<UserReportResponseModel>(
                "SELECT * FROM user.fn_UserReport(@p_flag, @p_userid);",
                paramobj,
                commandType: CommandType.Text);

            return result != null ? ApiResponse.Ok(result) : ApiResponse.Fail("Data not found");
        }
    }
}
