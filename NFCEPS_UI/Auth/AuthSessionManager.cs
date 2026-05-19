using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace NFCEPS_UI.Auth;

public class AuthSessionManager(IJSRuntime js, TokenStore tokenStore)
{
    public event Action? OnSessionChanged;
public event Action? OnSessionExpired;
    private const string Key = "authToken";

    public async Task<string?> GetTokenAsync()
    {
        if (!string.IsNullOrWhiteSpace(tokenStore.Token))
            return tokenStore.Token;

        if (js is not IJSInProcessRuntime)
        {
            // JS not ready yet → do NOT block auth pipeline
            return null;
        }

        try
        {
            var token = await js.InvokeAsync<string?>("localStorage.getItem", Key);

            if (!string.IsNullOrWhiteSpace(token))
                tokenStore.Set(token);

            return token;
        }
        catch
        {
            return null;
        }
    }

    public async Task LoginAsync(string token)
    {
        tokenStore.Set(token);// set in memory immediately
        await js.InvokeVoidAsync("localStorage.setItem", Key, token);
    }

public async Task LogoutAsync()
{
    tokenStore.Clear();

    try
    {
        await js.InvokeVoidAsync("localStorage.removeItem", Key);
    }
    catch { }

    OnSessionChanged?.Invoke();
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
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(jwt))
                return Enumerable.Empty<Claim>();

            return handler.ReadJwtToken(jwt).Claims;
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }
    public void MarkSessionExpired()
{
    tokenStore.Clear();
    OnSessionExpired?.Invoke();
}
}