using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.EmailJobAggregate.ValueObjects
{
    public class EmailJobId(Guid value) : ValueObject
    {
        public Guid Value { get; } = value;

        internal static EmailJobId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}