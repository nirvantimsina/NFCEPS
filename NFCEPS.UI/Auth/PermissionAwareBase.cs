// Components/Pages/PermissionAwareBase.cs
using Microsoft.AspNetCore.Components;
using NFCEPS.UI.Services;

namespace NFCEPS.UI.Components.Pages;

public abstract class PermissionAwareBase : ComponentBase, IDisposable
{
    [Inject] protected PermissionService PermissionService { get; set; } = default!;

    protected override void OnInitialized()
    {
        PermissionService.OnChange += OnPermissionsLoaded;
    }

    private void OnPermissionsLoaded()
    {
        InvokeAsync(async () =>
        {
            await OnPermissionsReadyAsync();
            StateHasChanged();
        });
    }

    // Override this in each page instead of LoadDataAsync
    protected virtual Task OnPermissionsReadyAsync() => Task.CompletedTask;

    public virtual void Dispose()
    {
        PermissionService.OnChange -= OnPermissionsLoaded;
    }
}

