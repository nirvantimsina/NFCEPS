using NFCEPS.UI.Models;

namespace NFCEPS.UI.Auth;

public class TokenStore
{
    private string? _token;

    public string? Token
    {
        get => _token;
        set => _token = value;
    }
    public List<MenuListModel> MenuList { get; set; } = new();
}

