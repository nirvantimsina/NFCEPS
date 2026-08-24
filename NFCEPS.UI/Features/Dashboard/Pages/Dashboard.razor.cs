using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Features.Dashboard.Managers.Interface;
using NFCEPS.UI.Features.Dashboard.Models.ResponseModel;
namespace NFCEPS.UI.Features.Dashboard.Pages;

public partial class Dashboard(IDashboardManager dashboardManager) : PermissionAwareBase
{
    public DashboardResponseModel? response;
    public bool IsLoading { get; private set; } = true;
    protected override async Task OnInitializedAsync()
    {
        await OnPermissionsReadyAsync();
    }
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



