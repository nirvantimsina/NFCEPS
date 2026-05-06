namespace NFCEPS_UI.Services;

public class PermissionService
{
    private HashSet<string> _permissions = [];

    public event Action? OnChange;

    public void SetPermissions(IEnumerable<string> permissions)
    {
        _permissions = new HashSet<string>(permissions);
        NotifyStateChanged();
    }

    public bool Has(string permKey)
        => _permissions.Contains(permKey);

    public void Clear()
    {
        _permissions.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
        => OnChange?.Invoke();
}