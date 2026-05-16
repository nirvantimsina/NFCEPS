using System.ComponentModel.DataAnnotations;

namespace NFCEPS_API.Auth.Models.Request;

public class LoginRequest
{

    [Required(ErrorMessage = "UserName is required.")]
    public string? UserName { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string? Password { get; set; }
}

