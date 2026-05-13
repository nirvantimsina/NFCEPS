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
    {
        try
        {
            return
            _permissions.Contains(permKey);
        }
        catch
        {
            return false;
        }
    }

    public bool HasAny(params string[] permKeys)
        => permKeys.Any(Has);

    public void Clear()
    {
        _permissions.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
        => OnChange?.Invoke();
}