using System.ComponentModel.DataAnnotations;

namespace NFCEPS_UI.Pages.Reports.Models.RequestModel
{
    public class UserReportRequestModel
    {
        [Required] public int UserId { get; set; }
    }
}


