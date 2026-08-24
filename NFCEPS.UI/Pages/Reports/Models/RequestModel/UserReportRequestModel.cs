using System.ComponentModel.DataAnnotations;

namespace NFCEPS.UI.Pages.Reports.Models.RequestModel
{
    public class UserReportRequestModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        public int? UserId { get; set; }
    }
}


