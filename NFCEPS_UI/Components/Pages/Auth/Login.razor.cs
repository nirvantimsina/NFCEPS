using Microsoft.AspNetCore.Components;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Auth.Managers;
using NFCEPS_UI.Models.Auth.RequestModel;
using Microsoft.AspNetCore.Components.Web;
using NFCEPS_UI.Models.Auth.ResponseModel;
using MudBlazor;
using System.Security.Claims;

namespace NFCEPS_UI.Components.Pages.Auth
{
    public partial class LoginBase : ComponentBase
    {
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
            var token = await Session.GetTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                Navigation.NavigateTo("/dashboard");
            }
        }

        protected async Task SignIn()
        {
            var result = await AuthManager.LoginAsync(request);

            if (result?.Success != true)
            {
                Snackbar.Add(result?.Message ?? "Login failed", Severity.Error);
                return;
            }

            var token = result.Data!.Token;

            await Session.LoginAsync(token);

            var claims = await Session.GetClaimsAsync();

            if (!claims.Any())
            {
                Snackbar.Add("Invalid token received", Severity.Error);
                return;
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            AuthStateProvider.SetAuthenticated(user);

            await Task.Delay(50);

            await PermissionService.LoadFromTokenAsync(Session);

            Navigation.NavigateTo("/dashboard");
        }

        protected async Task HandleKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SignIn();
            }
        }
    }
}