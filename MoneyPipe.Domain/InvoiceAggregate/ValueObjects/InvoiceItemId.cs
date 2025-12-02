using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.InvoiceAggregate.ValueObjects
{
    public class InvoiceItemId:ValueObject
    {
        public Guid Value { get;}

        private InvoiceItemId(Guid value)
        {
            Value = value;
        }

        internal static InvoiceItemId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}