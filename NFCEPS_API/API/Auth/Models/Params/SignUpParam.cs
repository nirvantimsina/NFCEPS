namespace NFCEPS_API.API.Auth.Models.Params
{
    public class SignUpParam
    {
        public string? Flag { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public byte[]? Password { get; set; }
    }
}