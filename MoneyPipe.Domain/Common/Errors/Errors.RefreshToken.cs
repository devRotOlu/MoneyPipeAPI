using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class RefreshToken
        {
            public static Error InvalidToken => Error.Unauthorized(code: "RefreshToken.Unauthorized", description: "Invalid refresh token");
        }
    }
}
