using NFCEPS_UI.Auth;

namespace NFCEPS_UI.Services;

public class PermissionService
{
    private readonly object _lock = new();
    private HashSet<string> _permissions = [];

    public event Action? OnChange;

    public async Task LoadFromTokenAsync(AuthSessionManager session)
    {
        var claims = await session.GetClaimsAsync();

        var permissions = claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value);

        SetPermissions(permissions);
    }

    public bool Has(string permKey)
        => !string.IsNullOrEmpty(permKey) && _permissions.Contains(permKey);

    public bool HasAny(params string[] permKeys)
        => permKeys.Any(Has);

    private void NotifyStateChanged()
        => OnChange?.Invoke();

    public void SetPermissions(IEnumerable<string> permissions)
    {
        lock (_lock)
        {
            _permissions = new HashSet<string>(permissions);
        }

        NotifyStateChanged();
    }

    public void Clear()
    {
        lock (_lock)
        {
            _permissions.Clear();
        }

        NotifyStateChanged();
    }

}

