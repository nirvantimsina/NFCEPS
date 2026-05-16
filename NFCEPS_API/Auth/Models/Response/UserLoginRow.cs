namespace NFCEPS_API.Auth.Models.Response;

public class UserLoginRow
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? Name {get; set;}
    public byte[] Password { get; set; } = [];
    public bool IsActive { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? CompressedPermissions { get; set; }
}