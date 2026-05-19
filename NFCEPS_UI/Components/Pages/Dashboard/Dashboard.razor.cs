using Microsoft.AspNetCore.Components;
using NFCEPS_UI.Managers.Dashboard.Interface;
using NFCEPS_UI.Models.Dashboard.ResponseModel;

namespace NFCEPS_UI.Components.Pages.Dashboard;

public partial class Dashboard(
    IDashboardManager dashboardManager) : ComponentBase
{
    [Inject] private PermissionService PermissionService { get; set; } = default!;
    public DashboardResponseModel? response;
    public bool IsLoading { get; private set; } = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            var result = await dashboardManager.DashboardDataAsync();

            response = result?.Success == true ? result.Data : null;
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