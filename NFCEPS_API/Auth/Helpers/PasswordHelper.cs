namespace NFCEPS_API.Auth;

public class PasswordHelper
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string Password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(Password, storedHash);
    }
}