using NFCEPS.UI.Shared.Security;
using NFCEPS.UI.Shared.Infrastructure;

namespace NFCEPS.UI.Features.Auth;

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




