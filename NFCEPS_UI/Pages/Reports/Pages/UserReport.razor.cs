using NFCEPS_UI.Components.Pages;
using NFCEPS_UI.Pages.Reports.Managers.Interface;
using NFCEPS_UI.Pages.Reports.Models.ResponseModel;

namespace NFCEPS_UI.Pages.Reports.Pages
{
    public partial class UserReport(IUserReportManager userReportManager) : PermissionAwareBase
    {
        public UserReportResponseModel? response;
        public bool IsLoading { get; private set; } = true;
        protected override async Task OnInitializedAsync()
        {
            await OnPermissionsReadyAsync();
        }

        protected override async Task OnPermissionsReadyAsync()
        {
            await LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                var result = await userReportManager.UserReportDataAsync();

                if (result?.Success == true && result.Data is not null)
                    response = result.Data;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}