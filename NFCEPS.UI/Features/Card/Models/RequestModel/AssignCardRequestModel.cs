using NFCEPS.UI.Shared.Security;
using System.ComponentModel.DataAnnotations;

namespace NFCEPS.UI.Features.Card.Models.RequestModel;

public class AssignCardRequestModel
{
    [Required]
    public int UserId { get; set; }
}





