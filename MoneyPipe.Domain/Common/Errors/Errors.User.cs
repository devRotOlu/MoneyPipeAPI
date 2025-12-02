using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateEmail => Error.Conflict(code: "User.DuplicateEmail", description: "Email already in use.");
            public static Error NotFound => Error.NotFound(code: "User.NotFound", description: "User isn't registered.");
            public static Error EmailRequired => Error.Validation(code:"User.EmailRequired",description:"Email address is required.");
            public static Error FirstRequired => Error.Validation(code:"User.FirstRequired",description:"First name is required.");
            public static Error LastRequired => Error.Validation(code:"User.LastRequired",description:"Last name is required.");
            public static Error PasswordRequired => Error.Validation(code:"User.PasswordRequired",description:"Password is required.");
        }
    }
}
