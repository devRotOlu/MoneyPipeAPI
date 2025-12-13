using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.UserAggregate.ValueObjects
{
    public sealed class RefreshTokenId: ValueObject
    {
        public Guid Value { get;}

        private RefreshTokenId(Guid value)
        {
            Value = value;
        }

        internal static RefreshTokenId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}