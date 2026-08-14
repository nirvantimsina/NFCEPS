using NFCEPS_API.Wrapper;
using NFCEPS_API.API.Dashboard.Models.RequestModel;

namespace NFCEPS_API.API.Dashboard.Services.Interface;

public interface IDashboardService
{
    Task<ApiResponse> GetDashboardDataAsync(DashboardRequestModel dashboardRequestModel);
}