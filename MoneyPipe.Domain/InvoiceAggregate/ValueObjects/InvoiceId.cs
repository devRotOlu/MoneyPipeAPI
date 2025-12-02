using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.InvoiceAggregate.ValueObjects
{
    public class InvoiceId : ValueObject
    {
        public Guid Value { get;}

        private InvoiceId(Guid value)
        {
            Value = value;
        }

        internal static InvoiceId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}