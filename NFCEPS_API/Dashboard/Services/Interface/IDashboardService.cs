using NFCEPS_API.Wrapper;
using NFCEPS_API.Dashboard.Models.RequestModel;

namespace NFCEPS_API.Dashboard.Services.Interface;

public interface IDashboardService
{
    Task<ApiResponse> GetDashboardDataAsync(DashboardRequestModel dashboardRequestModel);
}