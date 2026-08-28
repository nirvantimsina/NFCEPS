namespace NFCEPS.Shared.Models
{
    public static class ErrorCodes
    {
        // Success
        public const string? Success = "0";
        
        // Generic/Legacy Error
        public const string? GeneralError = "1";

        #region Authentication Errors 1000s
        public const string? UserAlreadyExists = "1001";
        public const string? UserDoesNotExist = "1002";
        public const string? InvalidCredentials = "1003";
        public const string? AccountInactive = "1004";
        #endregion

        #region Card Errors 2000s
        public const string? CardAlreadyAssigned = "2001";
        public const string? CardDoesNotExist = "2002";
        public const string? InsufficientBalance = "2003";
        public const string? CardExpired = "2004";
        #endregion

        #region System Errors 3000s
        public const string? Unauthorized = "3001";
        public const string? RecordNotFound = "3002";
        #endregion

        #region Validation Errors 4000s
        public const string? MissingRequiredField = "4001";
        public const string? InvalidFormat = "4002";
        public const string? InvalidPasswordFormat = "4003";
        public const string? InvalidPhoneNoFormat = "4004";
        public const string? OnlyInteger = "4005";
        public const string? InvalidUsernameFormat = "4006";
        #endregion

        #region Hardware Errors 5000s

        #endregion

        public static string? GetMessage(string statusCode)
        {
            return statusCode switch
            {
                Success => "Success",
                GeneralError => "An error occurred.",

                #region Auth Errors
                UserAlreadyExists => "Username or Phone Number already exists.",
                UserDoesNotExist => "The specified user does not exist.",
                InvalidCredentials => "Invalid username or password.",
                #endregion

                #region Card Errors
                AccountInactive => "This account is inactive.",
                CardAlreadyAssigned => "This card has already been assigned to a user.",
                CardDoesNotExist => "The specified card does not exist.",
                InsufficientBalance => "Insufficient balance for this transaction.",
                CardExpired => "The assigned card has expired.",
                #endregion

                #region System Errors
                Unauthorized => "You are not authorized to perform this action.",
                RecordNotFound => "The requested record was not found.",
                #endregion

                #region User Errors
                MissingRequiredField => "A required field is missing.",
                InvalidFormat => "Invalid data format.",
                InvalidPasswordFormat => "Invalid password format.",
                InvalidPhoneNoFormat => "Phone number can only be of exactly 10 numbers!",
                OnlyInteger => "Field can only contain numbers!",
                InvalidUsernameFormat => "Username cannot contain spaces or special characters!",
                #endregion

                _ => null // Return null for unknown codes so we can fallback to the DB message
            };
        }
    }
}
