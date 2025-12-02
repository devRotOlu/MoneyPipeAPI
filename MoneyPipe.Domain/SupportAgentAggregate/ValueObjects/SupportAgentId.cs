using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.SupportAgentAggregate.ValueObjects
{
    public sealed class SupportAgentId : ValueObject
    {
        public Guid Value { get;}

        private SupportAgentId(Guid value)
        {
            Value = value;
        }

        internal static SupportAgentId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}