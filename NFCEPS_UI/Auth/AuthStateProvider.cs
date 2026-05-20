using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class AuthStateProvider : AuthenticationStateProvider
{
    public event Action? OnAuthStateChanged;

    private AuthenticationState _state =
        new(new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>(), "jwt")));

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_state);
    }

    public void SetAuthenticated(ClaimsPrincipal user)
    {
        _state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(_state));
        OnAuthStateChanged?.Invoke();
    }

    public void Logout()
    {
        _state = new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity()));

        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }
}