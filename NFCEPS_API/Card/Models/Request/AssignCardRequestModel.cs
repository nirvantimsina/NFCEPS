using System.ComponentModel.DataAnnotations;

namespace NFCEPS_API.Card.Models.Request
{
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
}