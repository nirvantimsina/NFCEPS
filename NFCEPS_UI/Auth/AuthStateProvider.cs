using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace NFCEPS_UI.Auth;

public class AuthStateProvider(AuthSessionManager session) : AuthenticationStateProvider
{
    private AuthenticationState _state =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? ""));
    }

    public void MarkUserAsAuthenticatedWithToken(string token)
    {
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        _state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
public override async Task<AuthenticationState> GetAuthenticationStateAsync()
{
    if (_state.User.Identity?.IsAuthenticated != true)
    {
        var token = await session.GetTokenAsync(); // now properly reads localStorage
        if (!string.IsNullOrEmpty(token))
        {
            SetUser(ParseClaimsFromJwt(token));
        }
    }
    return _state;
}

    public void SetUser(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        _state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }

    public void MarkUserAsAuthenticated(string userName)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "apiauth");
        var user = new ClaimsPrincipal(identity);

        // FIX: Update the internal state so GetAuthenticationStateAsync returns the new user
        _state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }


    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void Logout()
    {
        _state =
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }
}