using System.Security.Claims;
using NFCEPS_UI.Auth;

public class AuthService
{
    private readonly AuthSessionManager _session;
    private readonly AuthStateProvider _provider;
    private readonly PermissionService _permissions;

    public AuthService(AuthSessionManager s, AuthStateProvider p, PermissionService ps)
    {
        _session = s;
        _provider = p;
        _permissions = ps;
    }

    public async Task InitializeAsync()
    {
        var claims = await _session.GetClaimsAsync();

        var permissions = claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value);

        _permissions.SetPermissions(permissions);
    }

    public async Task ApplyLogin(string token, IEnumerable<Claim> claims)
    {
        await _session.LoginAsync(token);

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        _provider.SetAuthenticated(user);
    }

    public async Task Logout()
    {
        await _session.LogoutAsync();
        _provider.Logout();
    }
}