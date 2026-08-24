using System.ComponentModel.DataAnnotations;

namespace NFCEPS.UI.Pages.Card.Models.RequestModel;

public class AssignCardRequestModel
{
    [Required]
    public int UserId { get; set; }
}


