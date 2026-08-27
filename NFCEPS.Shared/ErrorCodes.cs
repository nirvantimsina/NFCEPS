namespace NFCEPS.Domain.Models
{
    public static class ErrorCodes
    {
        // 0: Success
        public const string? Success = "0";
        
        // 1: Generic/Legacy Error (Used currently)
        public const string? GeneralError = "1";

        // 1000s: Authentication & User Errors
        public const string? UserAlreadyExists = "1001";
        public const string? UserDoesNotExist = "1002";
        public const string? InvalidCredentials = "1003";
        public const string? AccountInactive = "1004";
        public const string? MissingRequiredField = "1005";

        // 2000s: Card & Hardware Errors
        public const string? CardAlreadyAssigned = "2001";
        public const string? CardDoesNotExist = "2002";
        public const string? InsufficientBalance = "2003";
        public const string? CardExpired = "2004";

        // 3000s: System & Validation Errors
        public const string? Unauthorized = "3001";
        public const string? RecordNotFound = "3002";

        public static string? GetMessage(string statusCode)
        {
            return statusCode switch
            {
                Success => "Success",
                GeneralError => "An error occurred.",
                UserAlreadyExists => "Username or Phone Number already exists.",
                UserDoesNotExist => "The specified user does not exist.",
                InvalidCredentials => "Invalid username or password.",
                AccountInactive => "This account is inactive.",
                MissingRequiredField => "A required field is missing.",
                CardAlreadyAssigned => "This card has already been assigned to a user.",
                CardDoesNotExist => "The specified card does not exist.",
                InsufficientBalance => "Insufficient balance for this transaction.",
                CardExpired => "The assigned card has expired.",
                Unauthorized => "You are not authorized to perform this action.",
                RecordNotFound => "The requested record was not found.",
                _ => null // Return null for unknown codes so we can fallback to the DB message
            };
        }
    }
}
