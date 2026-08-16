using NFCEPS_API.API.Reports.Models.RequestModel;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.API.Reports.Services.Interface
{
    public interface IUserReportService
    {
        Task<ApiResponse> GetUsersReportAsync(UserReportRequestModel userReportRequest);
    }
}
