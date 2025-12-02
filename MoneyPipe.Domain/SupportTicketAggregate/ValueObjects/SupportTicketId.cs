using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.SupportTicketAggregate.ValueObjects
{
    public sealed class SupportTicketId : ValueObject
    {
        public Guid Value { get;}

        private SupportTicketId(Guid value)
        {
            Value = value;
        }

        internal static SupportTicketId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}