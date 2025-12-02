using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class SupportAgent
        {
            public static Error InvalidName => Error.Validation(
                code: "SupportAgent.InvalidName",
                description: "A valid agent name must be provided.");

            public static Error InvalidEmail => Error.Validation(
                code: "SupportAgent.InvalidEmail",
                description: "A valid agent email address is required.");
            public static Error ConflictingRoles => Error.Conflict(
                code: "SupportAgent.ConflictingRoles",
                description: "The support agent cannot be assigned multiple roles that conflict with each other.");
        }
    }
}