using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NFCEPS_API.Wrapper;

namespace NFCEPS_API.Auth;

public class JWTHelper(JWTSettings settings)
{
    // Auth/JwtHelper.cs
    public string GenerateToken(int userId, string userName, int roleId)
    {
        if (string.IsNullOrEmpty(settings.SecretKey))
        {
            throw new InvalidOperationException("JWT Secret Key is not configured in appsettings.json.");
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(settings.SecretKey));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim("roleId", roleId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(settings.ExpiryHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}