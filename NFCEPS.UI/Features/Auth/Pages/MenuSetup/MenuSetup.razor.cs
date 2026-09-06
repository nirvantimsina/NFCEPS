using Microsoft.AspNetCore.Components;
using MudBlazor;
using NFCEPS.UI.Features.Auth.Managers.Interface;
using NFCEPS.UI.Shared.Infrastructure;
using NFCEPS.UI.Shared.Security;

namespace NFCEPS.UI.Features.Auth.Pages.MenuSetup
{
    public partial class MenuSetup(IMenuSetupManager manager, ISnackbar snackbar) : PermissionAwareBase
    {
        [Inject]
        protected NavigationManager nav { get; set; } = default!;
        protected MenuListModel request = new();
        protected string? error;
        protected bool IsLoading = false;

        protected async Task GetAllMenuList()
        {
            error = null;
            IsLoading = true;

            var result = await manager.GetAllMenuListAsync();

            if (result?.Success != true)
            {
                error = result?.Message ?? "Getting Menu List Faild";

                snackbar.Add(error, Severity.Error);
                return;
            }
            else
            {
                snackbar.Add(result.Message, Severity.Error);
            }
        }
    }
}