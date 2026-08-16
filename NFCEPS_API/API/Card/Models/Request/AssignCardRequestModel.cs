using System.ComponentModel.DataAnnotations;

namespace NFCEPS_API.API.Card.Models.Request
{
    public class AssignCardRequestModel
    {
        public string? Flag { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}