using NFCEPS_UI.Auth;

public class PermissionService
{
    private readonly object _lock = new();
    private HashSet<string> _permissions = new();

    public event Action? OnChange;

    public void SetPermissions(IEnumerable<string> permissions)
    {
        lock (_lock)
        {
            _permissions = new HashSet<string>(permissions);
        }

        NotifyStateChanged();
    }

    public async Task LoadFromTokenAsync(AuthSessionManager session)
    {
        var claims = await session.GetClaimsAsync();

        var permissions = claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value);

        SetPermissions(permissions);
    }

    public bool HasPermission(string permission)
        => !string.IsNullOrEmpty(permission) && _permissions.Contains(permission);

    public bool HasAnyPermission(params string[] permissions)
        => permissions.Any(HasPermission);

    public void Clear()
    {
        lock (_lock)
        {
            _permissions.Clear();
        }

        NotifyStateChanged();
    }

    private void NotifyStateChanged()
        => OnChange?.Invoke();
}