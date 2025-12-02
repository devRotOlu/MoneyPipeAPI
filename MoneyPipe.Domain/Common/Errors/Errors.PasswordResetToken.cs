using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class PasswordResetToken
        {
            public static Error InvalidToken => Error.Validation(
                code: "PasswordResetToken.InvalidToken",
                description: "Invalid token.");
            
            public static Error Expired => Error.Validation(
                code: "PasswordResetToken.Expired",
                description: "Token expired or already used.");
            
            public static Error TokenRequired => Error.Validation(
                code: "PasswordResetToken.TokenRequired",
                description: "Token creation failed because no token value was provided."); 
            
        }
    }
}