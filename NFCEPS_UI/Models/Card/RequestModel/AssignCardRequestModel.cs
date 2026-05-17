using System.ComponentModel.DataAnnotations;

namespace NFCEPS_UI.Models.Card.RequestModel;

public class AssignCardRequestModel
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public int CardId { get; set; }
    [Required]
    public int AvailableAmount { get; set; }
    [Required]
    public string? CheckSum { get; set; } // this is the checksum
}
