using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.UserAggregate.ValueObjects
{
    public sealed class PasswordRefreshTokenId: ValueObject
    {
        public Guid Value { get;}

        private PasswordRefreshTokenId(Guid value)
        {
            Value = value;
        }

        internal static PasswordRefreshTokenId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
        
}