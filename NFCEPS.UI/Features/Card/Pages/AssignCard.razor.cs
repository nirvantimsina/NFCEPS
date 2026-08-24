using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Features.Card.Managers.Interface;
using NFCEPS.UI.Features.Card.Models.RequestModel;

namespace NFCEPS.UI.Features.Card.Pages
{
    public partial class AssignCard(
    ICardManager cardManager,
    ISnackbar snackbar) : PermissionAwareBase
    {
        [Inject]
        protected NavigationManager nav { get; set; } = default!;
        protected AssignCardRequestModel request = new();
        protected string? error;
        protected bool IsLoading = false;

        protected override async Task OnPermissionsReadyAsync()
        {
            if (!PermissionService.Has("USR.C"))
            {
                nav.NavigateTo("unauthorized-page");
            }
            await Task.CompletedTask;
        }
        protected async Task HandleAssignCard()
        {
            error = null;
            IsLoading = true;

            try
            {
                var result = await cardManager.AssignCardAsync(request);

                if (result?.Success != true)
                {
                    error = result?.Message ?? "Assigning Card Faild";

                    snackbar.Add(error, Severity.Error);
                    return;
                }

                snackbar.Add("User has been assigned a card.", Severity.Success);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task HandleKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await HandleAssignCard();
            }
        }
    }
}




