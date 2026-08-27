namespace NFCEPS.Domain.Models
{
    public static class ErrorCodes
    {
        // 0: Success
        public const int Success = 0;
        
        // 1: Generic/Legacy Error (Used currently)
        public const int GeneralError = 1;

        // 1000s: Authentication & User Errors
        public const int UserAlreadyExists = 1001;
        public const int UserDoesNotExist = 1002;
        public const int InvalidCredentials = 1003;
        public const int AccountInactive = 1004;
        public const int MissingRequiredField = 1005;

        // 2000s: Card & Hardware Errors
        public const int CardAlreadyAssigned = 2001;
        public const int CardDoesNotExist = 2002;
        public const int InsufficientBalance = 2003;
        public const int CardExpired = 2004;

        // 3000s: System & Validation Errors
        public const int Unauthorized = 3001;
        public const int RecordNotFound = 3002;

        public static string? GetMessage(int statusCode)
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
