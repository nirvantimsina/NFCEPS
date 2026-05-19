using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NFCEPS_API.Auth;

public class JWTHelper(JWTSettings settings)
{
    // Auth/JwtHelper.cs
    public string GenerateToken(int userId, string userName, int roleId, IEnumerable<string> permissions)
    {
        if (string.IsNullOrEmpty(settings.SecretKey))
            throw new InvalidOperationException("JWT Secret Key is not configured in appsettings.json.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, userName),
            new("roleId", roleId.ToString())
        };

        // Add each permission as its own claim
        foreach (var permission in permissions)
            claims.Add(new Claim("permission", permission));

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