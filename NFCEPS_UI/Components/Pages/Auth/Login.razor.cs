using Microsoft.AspNetCore.Components;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Auth.Managers;
using NFCEPS_UI.Services;
using NFCEPS_UI.Models.Auth.RequestModel;
using Microsoft.AspNetCore.Components.Web;
using NFCEPS_UI.Models.Auth.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Components.Pages.Auth
{
    [AllowAnonymous]
    public partial class LoginBase : ComponentBase
    {
        [Inject] protected IAuthManager AuthManager { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected AuthStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] protected PermissionService PermissionService { get; set; } = default!;
        [Inject] protected CurrentUser CurrentUser { get; set; } = default!;
        [Inject] protected AuthSessionManager Session { get; set; } = default!;

        protected LoginRequest request { get; set; } = new();

        protected string? error;
        protected bool IsLoading = false;

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
                    return;
                }

                var data = result.Data;

                if (data == null || string.IsNullOrWhiteSpace(data.Token))
                {
                    error = "Invalid response from server";
                    return;
                }

                // Only call LoginAsync ONCE
                await Session.LoginAsync(data.Token);

                if (data.Permissions != null)
                    PermissionService.SetPermissions(data.Permissions);

                AuthStateProvider.MarkUserAsAuthenticatedWithToken(data.Token);

                Navigation.NavigateTo("/Dashboard");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                error = "Something went wrong!";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}