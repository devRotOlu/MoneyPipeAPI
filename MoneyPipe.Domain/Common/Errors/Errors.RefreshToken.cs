using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class RefreshToken
        {
            public static Error InvalidToken => Error.Unauthorized(
                code: "RefreshToken.Unauthorized", 
                description: "Invalid refresh token");
            public static Error TokenRequired => Error.Validation(
                code: "RefreshToken.TokenRequired",
                description: "Token creation failed because no token value was provided.");

            public static Error InvalidTokenExpiration => Error.Validation(
                code: "RefreshToken.InvalidTokenExpiration",
                description: "Token expiration must be a valid future time during creation.");  
        }
    }
}
