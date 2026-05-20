namespace NFCEPS_UI.Auth;

public class TokenStore
{
    private string? _token;

    public string? Token
    {
        get => _token;
        set => _token = value;
    }
}