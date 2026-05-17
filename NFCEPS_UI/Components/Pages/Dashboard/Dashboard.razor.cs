using Microsoft.AspNetCore.Components;
using NFCEPS_UI.Managers.Dashboard.Interface;
using NFCEPS_UI.Models.Dashboard.ResponseModel;

namespace NFCEPS_UI.Components.Pages.Dashboard;
public partial class Dashboard(
    IDashboardManager dashboardManager,
    AuthGuard authGuard,
    NavigationManager navigation,
    AuthService authService)
{
    [Inject] private PermissionService permissionService { get; set; } = default!;
    public DashboardResponseModel? response;
    public bool IsLoading { get; private set; } = true;

    protected override async Task OnInitializedAsync()
    {
        if (!await authGuard.IsAuthenticatedAsync())
        {
            navigation.NavigateTo("/login");
            return;
        }

        await authService.InitializeAsync();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            var result = await dashboardManager.DashboardDataAsync();

            if (result?.Success == true)
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