namespace NFCEPS_UI.Auth;

public class TokenStore
{
    public string? Token { get; private set; }

    public event Action? OnTokenChanged;

    public void Set(string token)
    {
        Token = token;
        OnTokenChanged?.Invoke();
    }

    public void Clear()
    {
        Token = null;
        OnTokenChanged?.Invoke();
    }
}