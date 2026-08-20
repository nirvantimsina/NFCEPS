using System.ComponentModel.DataAnnotations;

namespace NFCEPS_UI.Pages.Card.Models.RequestModel;

public class AssignCardRequestModel
{
    [Required]
    public int UserId { get; set; }
}


