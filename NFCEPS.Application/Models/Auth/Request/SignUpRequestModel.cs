using System.ComponentModel.DataAnnotations;

namespace NFCEPS.Application.Models.Auth.Request
{
    public class SignUpRequestModel
    {
        [Required(ErrorMessage = "UserName is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username cannot contain spaces or special characters.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string? Password { get; set; }
    }
}

