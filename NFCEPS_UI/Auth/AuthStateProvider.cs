using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace NFCEPS_UI.Auth;

public class AuthStateProvider : AuthenticationStateProvider
{
    private AuthenticationState _state =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(_state);

    public void SetUser(IEnumerable<Claim> claims)
    {
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        _state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }

    public void Logout()
    {
        _state =
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }
}