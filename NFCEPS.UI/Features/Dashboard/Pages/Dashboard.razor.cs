using MudBlazor;
using NFCEPS.Shared.Wrappers;
using NFCEPS.UI.Features.Dashboard.Managers.Interface;
using NFCEPS.UI.Features.Dashboard.Models.ResponseModel;
using NFCEPS.UI.Shared.Security;

namespace NFCEPS.UI.Features.Dashboard.Pages;

public partial class Dashboard(IDashboardManager dashboardManager, ISnackbar snackbar)
    : PermissionAwareBase
{
    public DashboardResponseModel? response;
    public bool IsLoading { get; private set; } = true;

    public string role = string.Empty;

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

    protected async Task OnClickTest()
    {
        snackbar.Add(ErrorCodes.CardAlreadyAssigned);
        snackbar.Add(ErrorCodes.GetMessage(ErrorCodes.CardAlreadyAssigned));
    }
}
