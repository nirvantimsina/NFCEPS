namespace NFCEPS_API.API.Auth.Helpers;

public class JWTSettings
{
    public string? SecretKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiryHours { get; set; }
}