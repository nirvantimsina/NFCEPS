using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NFCEPS_UI.Auth;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthSessionManager _session;
    private readonly PermissionService _permissionService;

    private AuthenticationState _state;

    public event Action? OnAuthStateChanged;

    public AuthStateProvider(
        AuthSessionManager session,
        PermissionService permissionService)
    {
        _session = session;
        _permissionService = permissionService;

        _state = new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _session.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            _state = new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));

            return _state;
        }

        var claims = await _session.GetClaimsAsync();

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        var permissions = claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value);

        _permissionService.SetPermissions(permissions);

        _state = new AuthenticationState(user);

        return _state;
    }

    public void SetAuthenticated(ClaimsPrincipal user)
    {
        var permissions = user.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value);

        _permissionService.SetPermissions(permissions);

        _state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(
            Task.FromResult(_state));

        OnAuthStateChanged?.Invoke();
    }

    public void Logout()
    {
        _permissionService.SetPermissions([]);

        _state = new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity()));

        NotifyAuthenticationStateChanged(
            Task.FromResult(_state));

        OnAuthStateChanged?.Invoke();
    }
}