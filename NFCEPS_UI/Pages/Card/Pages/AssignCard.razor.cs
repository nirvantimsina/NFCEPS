using MudBlazor;
using Microsoft.AspNetCore.Components.Web;
using NFCEPS_UI.Pages.Card.Managers.Interface;
using NFCEPS_UI.Components.Pages;
using NFCEPS_UI.Pages.Card.Models.RequestModel;
using Microsoft.AspNetCore.Components;

namespace NFCEPS_UI.Pages.Card.Pages
{
    public partial class AssignCard(
    ICardManager cardManager,
    ISnackbar snackbar) : PermissionAwareBase
    {
        protected AssignCardRequestModel request = new();
        protected string? error;
        protected bool IsLoading = false;
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
