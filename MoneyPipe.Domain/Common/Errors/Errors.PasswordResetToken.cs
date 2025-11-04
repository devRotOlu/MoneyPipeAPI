using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class PasswordResetToken
        {
            public static Error InvalidToken => Error.Validation(
                code: "Reset.InvalidToken",
                description: "Invalid token.");
            
            public static Error Expired => Error.Validation(
                code: "Reset.Expired",
                description: "Token expired or already used.");
        }
    }
}