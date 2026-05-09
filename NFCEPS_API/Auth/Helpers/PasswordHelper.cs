namespace NFCEPS_API.Auth;

public class PasswordHelper
{
    public static byte[] HashPassword(string Password)
    {
        string hashed = BCrypt.Net.BCrypt.HashPassword(Password);
        return System.Text.Encoding.UTF8.GetBytes(hashed);
    }

    public static bool VerifyPassword(string Password, byte[] storedHash)
    {
        string storedHashString = System.Text.Encoding.UTF8.GetString(storedHash);
        return BCrypt.Net.BCrypt.Verify(Password, storedHashString);
    }
}