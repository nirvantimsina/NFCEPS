namespace NFCEPS_API.Auth.Models.Response;

public class UserLoginRow
{
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public int UserId { get; set; }
    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? RoleName { get; set; }
    public int RoleId  { get; set; }
    public string? CompressedPermissions { get; set; }
}
