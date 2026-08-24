using System.ComponentModel.DataAnnotations;

namespace NFCEPS.Application.Models.Auth.Request;

public class LoginRequest
{

    [Required(ErrorMessage = "UserName is required.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string? Password { get; set; }
}



