using Microsoft.AspNetCore.Components;
using MudBlazor;
using NFCEPS.UI.Features.Auth.Managers.Interface;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Shared.Security;

namespace NFCEPS.UI.Features.Auth.Pages.MenuSetup
{
    public partial class MenuSetup(IMenuSetupManager menuSetupManager, ISnackbar snackbar) : PermissionAwareBase
    {
        protected List<MenuListModel>? response = new();
        [Inject]
        protected NavigationManager nav { get; set; } = default!;
        protected MenuListModel request = new();
        protected string? error;
        protected bool IsLoading = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        protected async Task LoadDataAsync()
        {
            IsLoading = true;

            var result = await menuSetupManager.GetAllMenuListAsync();

            if (result?.Success == false)
            {
                snackbar.Add(result?.Message ?? "Failed to load data", Severity.Error);
                return;
            }
            else if (result?.Success == true && result.Data is not null)
            {
                response = result.Data;
            }
            
            IsLoading = false;
        }
    }
}