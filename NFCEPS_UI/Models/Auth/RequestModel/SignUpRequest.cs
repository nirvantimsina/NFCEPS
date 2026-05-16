using System.ComponentModel.DataAnnotations;

namespace NFCEPS_UI.Models.Auth.RequestModel
{
    public class SignUpRequest
    {

        [Required(ErrorMessage = "UserName is required.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username cannot contain spaces or special characters.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Length(10, 10, ErrorMessage = "Phone numbers must be exactly 10 digits long.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}