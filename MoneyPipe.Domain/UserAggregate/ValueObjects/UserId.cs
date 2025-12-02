using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.UserAggregate.ValueObjects
{
    public sealed class UserId : ValueObject
    {
        public Guid Value { get;}

        private UserId(Guid value)
        {
            Value = value;
        }

        internal static UserId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        internal static UserId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}