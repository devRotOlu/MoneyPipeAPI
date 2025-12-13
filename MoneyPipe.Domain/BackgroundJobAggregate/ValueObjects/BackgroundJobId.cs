using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.BackgroundJobAggregate.ValueObjects
{
    public sealed class BackgroundJobId : ValueObject
    {
        public Guid Value { get;}

        private BackgroundJobId(Guid value)
        {
            Value = value;
        }

        internal static BackgroundJobId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}