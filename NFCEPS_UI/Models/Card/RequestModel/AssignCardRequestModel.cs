using System.ComponentModel.DataAnnotations;

namespace NFCEPS_UI.Models.Card.RequestModel;

public class AssignCardRequestModel
{
    [Required]
    public int UserId { get; set; }
}
