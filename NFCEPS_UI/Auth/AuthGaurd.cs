using NFCEPS_UI.Auth;

public class AuthGuard
{
    private readonly AuthSessionManager _session;

    public AuthGuard(AuthSessionManager session)
    {
        _session = session;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _session.GetTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }
}