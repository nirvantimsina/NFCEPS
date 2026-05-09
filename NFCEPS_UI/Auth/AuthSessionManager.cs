using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace NFCEPS_UI.Auth;

public class AuthSessionManager
{
    private readonly IJSRuntime _js;
    private readonly AuthStateProvider _auth;

    private const string Key = "authToken";
    private string? _cachedToken;

    public AuthSessionManager(IJSRuntime js, AuthStateProvider auth)
    {
        _js = js;
        _auth = auth;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var token = await _js.InvokeAsync<string?>("localStorage.getItem", Key);

            if (!string.IsNullOrWhiteSpace(token))
            {
                _cachedToken = token;
                _auth.SetUser(Parse(token));
            }
        }
        catch (InvalidOperationException)
        {
            // Prerendering: JS is not available yet.
            // The app will call this again once it's interactive.
        }
    }


    public async Task LoginAsync(string token)
    {
        _cachedToken = token;

        await _js.InvokeVoidAsync("localStorage.setItem", Key, token);

        _auth.SetUser(Parse(token));
    }

    public async Task LogoutAsync()
    {
        _cachedToken = null;
        try
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", Key);
        }
        catch (InvalidOperationException)
        {
            // We are likely prerendering. We've cleared the cache, 
            // the JS call will happen again once interactive.
        }
        _auth.Logout();
    }


    public Task<string?> GetTokenAsync()
        => Task.FromResult(_cachedToken);

    private IEnumerable<Claim> Parse(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(jwt).Claims;
    }
}
