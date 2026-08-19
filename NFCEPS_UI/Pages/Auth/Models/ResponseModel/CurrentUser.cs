using NFCEPS_UI.Models.ResponseModel;

namespace NFCEPS_UI.Pages.Auth.Models.ResponseModel;

public class CurrentUser()
{
    public string? UserName { get; private set; }
    public string? Name { get; private set; }
    public string? RoleName { get; private set; }
    public int RoleId { get; private set; }

    public void Set(LoginResponse response)
    {
        Name = response.Name;
        RoleName = response.RoleName;
        RoleId = response.RoleId;
    }

    public void Clear()
    {
        Name = null;
        RoleName = null;
        RoleId = 0;
    }
}


