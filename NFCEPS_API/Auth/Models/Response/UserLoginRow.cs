namespace NFCEPS_API.Auth.Models.Response;

public class UserLoginRow
{
    public int userid { get; set; }
    public string? username { get; set; }
    public string? name { get; set; }
    public string password { get; set; } = string.Empty;
    public bool isactive { get; set; }
    public int roleid { get; set; }
    public string? rolename { get; set; }
    public string? compressedpermissions { get; set; }
}
