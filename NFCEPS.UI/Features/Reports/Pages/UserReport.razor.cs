using Microsoft.AspNetCore.Components;
using MudBlazor;
using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Features.Reports.Managers.Interface;
using NFCEPS.UI.Features.Reports.Models.RequestModel;
using NFCEPS.UI.Features.Reports.Models.ResponseModel;

namespace NFCEPS.UI.Features.Reports.Pages
{
    public partial class UserReport(IUserReportManager userReportManager) : PermissionAwareBase
    {
        protected List<UserReportResponseModel>? response = new();
        protected UserReportRequestModel request = new();
        [Inject] ISnackbar snackbar { get; set; } = default!;
        public bool IsLoading { get; private set; } = false;
        protected override async Task OnInitializedAsync()
        {
            //if (!PermissionService.Has("USR.C"))
            //{
            //    nav.NavigateTo("unauthorized-page");
            //}
        }
        private async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var result = await userReportManager.UserReportDataAsync(request);

                if (result?.Success == true && result.Data is not null)
                    response = result.Data;
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
    }
}



