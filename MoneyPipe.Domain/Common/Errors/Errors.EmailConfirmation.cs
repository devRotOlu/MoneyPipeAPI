using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class EmailConfirmation
        {
            public static Error TokenExpired => Error.Validation(
                code: "EmailConfirmation.TokenExpired",
                description: "The email confirmation token has expired.");

            public static Error TokenMismatch => Error.Validation(
                code: "EmailConfirmation.TokenMismatch",
                description: "The email confirmation token does not match.");

            public static Error AlreadyConfirmed => Error.Conflict(
                code: "EmailConfirmation.AlreadyConfirmed",
                description: "The email has already been confirmed.");
            public static Error UnConfirmed => Error.Unauthorized(
                code: "EmailConfirmation.UnConfirmed",
                description: "The signup email isn't confirmed.");
        }
    }

}
