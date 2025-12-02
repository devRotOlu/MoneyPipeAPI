using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.SupportTicketAggregate.ValueObjects
{
    public sealed class SupportTicketMessageId : ValueObject
    {
        public Guid Value { get; set; }

        private SupportTicketMessageId(Guid value)
        {
            Value = value;
        }

        internal static SupportTicketMessageId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}