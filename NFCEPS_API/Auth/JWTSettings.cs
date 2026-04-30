namespace NFCEPS_API.Auth;

public class JWTSettings
{
    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiryHours { get; set; }
}