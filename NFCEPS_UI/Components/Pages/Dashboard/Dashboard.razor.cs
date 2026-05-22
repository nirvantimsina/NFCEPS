using NFCEPS_UI.Managers.Dashboard.Interface;
using NFCEPS_UI.Models.Dashboard.ResponseModel;

namespace NFCEPS_UI.Components.Pages.Dashboard;

public partial class Dashboard(IDashboardManager dashboardManager) : PermissionAwareBase
{
    public DashboardResponseModel? response;
    public bool IsLoading { get; private set; } = true;
    protected override async Task OnPermissionsReadyAsync()
    {
        await LoadDataAsync();
    }
    private async Task LoadDataAsync()
    {
        IsLoading = true;
        try
        {
            var result = await dashboardManager.DashboardDataAsync();
            Console.WriteLine($"Success: {result?.Success}, Name: {result?.Data?.Name}");

            if (result?.Success == true && result.Data is not null)
                response = result.Data;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Dashboard error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}