using Microsoft.AspNetCore.Components;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Managers.Dashboard.Interface;
using NFCEPS_UI.Models.Dashboard.ResponseModel;
using NFCEPS_UI.Services;

namespace NFCEPS_UI.Components.Pages.Dashboard;

public partial class Dashboard(
    PermissionService permissionService,
    IDashboardManager dashboardManager,
    AuthSessionManager authSessionManager,
    NavigationManager navigation)
{
    public DashboardResponseModel? response;
    public bool IsLoading { get; private set; } = true;

 protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (!firstRender) return;

    var token = await authSessionManager.GetTokenAsync();
    Console.WriteLine($"Token in dashboard OnAfterRender: {(string.IsNullOrEmpty(token) ? "NULL" : "found")}");

    if (string.IsNullOrWhiteSpace(token))
    {
        navigation.NavigateTo("/login", forceLoad: true);
        return; // ← make sure you return here and don't call LoadDataAsync
    }



    await LoadDataAsync();
    StateHasChanged();
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