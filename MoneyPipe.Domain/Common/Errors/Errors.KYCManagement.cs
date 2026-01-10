using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class KYCManagement
        {
            public static Error MissingProfile => Error.NotFound(code: "Profile.MissingProfile", description: "KYC profile is incomplete. Please provide all required personal details to proceed");
        }
    }
}