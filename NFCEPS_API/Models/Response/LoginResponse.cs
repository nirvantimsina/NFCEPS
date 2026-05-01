namespace NFCEPS_API.Models.Response;

public class LoginResponse
{
    public string? Token { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? RoleName { get; set; }
    public int RoleId  { get; set; }
    public List<string> Permissions { get; set; } = [];
}