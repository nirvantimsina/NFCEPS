using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace NFCEPS_UI.Auth;

public class AuthSessionManager(IJSRuntime js, TokenStore tokenStore)
{
    private const string Key = "authToken";

    public async Task<string?> GetTokenAsync()
    {
        // Return from memory first
        if (!string.IsNullOrWhiteSpace(tokenStore.Token))
            return tokenStore.Token;

        // Fall back to localStorage (page reload scenario)
        try
        {
            var token = await js.InvokeAsync<string?>("localStorage.getItem", Key);
            if (!string.IsNullOrWhiteSpace(token))
            {
                tokenStore.Token = token; // cache in memory for this circuit
                return token;
            }
        }
        catch (InvalidOperationException) { }

        return null;
    }

    public async Task LoginAsync(string token)
    {
        tokenStore.Token = token; // set in memory immediately
        await js.InvokeVoidAsync("localStorage.setItem", Key, token);
    }

    public async Task LogoutAsync()
    {
        tokenStore.Token = null;
        try
        {
            await js.InvokeVoidAsync("localStorage.removeItem", Key);
        }
        catch (InvalidOperationException) { }
    }

    public async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return Enumerable.Empty<Claim>();
        return Parse(token);
    }

    private IEnumerable<Claim> Parse(string jwt)
    {
        try
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims;
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }
}