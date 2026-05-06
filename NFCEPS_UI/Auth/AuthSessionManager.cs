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
        var token = await _js.InvokeAsync<string?>(Key, "getItem");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _cachedToken = token;
            _auth.SetUser(Parse(token));
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

        await _js.InvokeVoidAsync("localStorage.removeItem", Key);

        _auth.Logout();
    }

    public Task<string?> GetTokenAsync()
        => Task.FromResult(_cachedToken);

    private IEnumerable<Claim> Parse(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(jwt).Claims;
    }
