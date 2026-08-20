using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using NFCEPS_UI.Pages.Auth.Managers.Interface;
using NFCEPS_UI.Pages.Auth.Models.RequestModel;

namespace NFCEPS_UI.Pages.Auth.Pages
{
    [AllowAnonymous]
    public partial class SignUp(
        NavigationManager Navigation,
        IAuthManager AuthManager,
        ISnackbar Snackbar) : ComponentBase
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStateTask { get; set; } = default!;
        protected SignUpRequest request = new();
        protected string? error;
        protected bool IsLoading = false;
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateTask;

            if (authState.User.Identity is { IsAuthenticated: true })
            {
                Navigation.NavigateTo("dashboard");
            }
        }
        protected async Task HandleSignUp()
        {
            error = null;
            IsLoading = true;

            try
            {
                var result = await AuthManager.SignUpAsync(request);

                if (result == null || !result.Success)
                {
                    error = result?.Message ?? "Signup Failed!";

                    Snackbar.Add(error, Severity.Error);
                    return;
                }

                Snackbar.Add("Account created successfully!", Severity.Success);
                Navigation.NavigateTo("/login");
            }
            catch
            {
                error = "Something went wrong";
                Snackbar.Add(error, Severity.Error);
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
                await HandleSignUp();
            }
        }
    }
}

