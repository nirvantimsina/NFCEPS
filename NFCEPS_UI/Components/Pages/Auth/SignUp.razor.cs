using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using NFCEPS_UI.Auth.Managers;
using NFCEPS_UI.Models.Auth.RequestModel;

namespace NFCEPS_UI.Components.Pages.Auth
{
    public partial class SignUp(
        NavigationManager Navigation,
        IAuthManager AuthManager,
        ISnackbar Snackbar) : ComponentBase
    {
        protected SignUpRequest request = new();
        protected string? error;
        protected bool IsLoading = false;
        protected async Task HandleSignUp()
        {
            error = null;
            IsLoading = true;

            try
            {
                var result = await AuthManager.SignUpAsync(request);

                if (result?.Success != true)
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
            if(e.Key == "Enter")
            {
                await HandleSignUp();
            }
        }
    }
}