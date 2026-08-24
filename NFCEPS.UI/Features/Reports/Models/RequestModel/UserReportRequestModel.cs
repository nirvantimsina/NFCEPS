using NFCEPS.UI.Shared.Security;
using System.ComponentModel.DataAnnotations;

namespace NFCEPS.UI.Features.Reports.Models.RequestModel
{
    public class UserReportRequestModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        public int? UserId { get; set; }
    }
}





