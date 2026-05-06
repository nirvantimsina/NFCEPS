using Microsoft.AspNetCore.Components;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Managers;
using NFCEPS_UI.Services;
using NFCEPS_UI.Models.RequestModel;

namespace NFCEPS_UI.Components.Pages
{
    public partial class LoginBase : ComponentBase
    {
        [Inject] protected IAuthManager AuthManager { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }
        [Inject] protected AuthStateProvider AuthStateProvider { get; set; }
        [Inject] protected PermissionService PermissionService { get; set; }
        [Inject] protected CurrentUser CurrentUser { get; set; }
        [Inject] protected AuthSessionManager Session { get; set; }

        protected LoginRequest LoginModel { get; set; } = new();

        protected string _error;
        protected bool _loading = false;

        protected async Task Login()
        {
            _error = null;
            _loading = true;

            try
            {
                var result = await AuthManager.LoginAsync(LoginModel);

                if ( result == null || !result.Success)
                {
                    _error = result?.Message ?? "Login Failed";
                    return;
                }

                var data = result.Data;

                if (data == null || string.IsNullOrWhiteSpace(data.Token))
                {
                    _error = "Invalid Response from the server";
                    return;
                }

                await Session.LoginAsync(result.Data.Token);

                PermissionService.SetPermissions(result.Data.Permissions);

                CurrentUser.Set(data);

                Navigation.NavigateTo("/", true);
            }

            catch
            {
                _error = "Something went wrong!";
            }

            finally
            {
                _loading = false;
            }
        }
    }
}