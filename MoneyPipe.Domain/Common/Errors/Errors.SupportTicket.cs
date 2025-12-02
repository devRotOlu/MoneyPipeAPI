using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class SupportTicket
        {
            public static Error DescriptionRequired => Error.Validation(code:"SupportTicket.DescriptionRequired",description:"Description is required to raise a SupportTicket");
            public static Error UserIdRequired => Error.Validation(code:"SupportTicket.UserIdRequired",description:"User's identifier is required to raise a SupportTicket");
        }
    }
}