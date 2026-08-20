using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Pages.Auth.Managers.Interface;
using NFCEPS_UI.Pages.Auth.Models.RequestModel;
using NFCEPS_UI.Pages.Auth.Models.ResponseModel;
using NFCEPS_UI.Services;

namespace NFCEPS_UI.Pages.Auth.Pages
{
    [AllowAnonymous]
    public partial class LoginBase : ComponentBase
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStateTask { get; set; } = default!;
        [Inject] protected IAuthManager AuthManager { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected AuthStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] protected PermissionService PermissionService { get; set; } = default!;
        [Inject] protected CurrentUser CurrentUser { get; set; } = default!;
        [Inject] protected AuthSessionManager Session { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected LoginRequest request { get; set; } = new();
        protected string? error;
        protected bool IsLoading = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateTask;

            // If the user is already logged in, "bounce" them to the dashboard
            if (authState.User.Identity is { IsAuthenticated: true })
            {
                Navigation.NavigateTo("/dashboard");
            }
        }
        protected async Task HandleKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SignIn();
            }
        }

        protected async Task SignIn()
        {
            error = null;
            IsLoading = true;

            try
            {
                var result = await AuthManager.LoginAsync(request);

                if (result == null || !result.Success)
                {
                    error = result?.Message ?? "Login Failed";
                    Snackbar.Add(error, Severity.Error);
                    return;
                }

                var data = result.Data;

                if (data == null || string.IsNullOrWhiteSpace(data.Token))
                {
                    error = "Invalid response from server";
                    Snackbar.Add(error, Severity.Error);
                    return;
                }

                // Only call LoginAsync ONCE
                await Session.LoginAsync(data.Token, data.MenuList);

                if (data.Permissions != null)
                    PermissionService.SetPermissions(data.Permissions);

                AuthStateProvider.MarkUserAsAuthenticatedWithToken(data.Token);

                Navigation.NavigateTo("/dashboard");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                error = "Something went wrong!";
                Snackbar.Add(error, Severity.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}

