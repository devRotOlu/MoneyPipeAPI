using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.WalletAggregate.ValueObjects
{
    public sealed class WalletId:ValueObject
    {
        public Guid Value { get;}

        private WalletId(Guid value)
        {
            Value = value;
        }

        internal static WalletId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        internal static WalletId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}