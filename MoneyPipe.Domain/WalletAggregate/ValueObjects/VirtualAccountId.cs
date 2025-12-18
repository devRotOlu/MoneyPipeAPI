using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.WalletAggregate.ValueObjects
{
    public sealed class VirtualAccountId:ValueObject
    {
        public Guid Value { get;}

        private VirtualAccountId(Guid value)
        {
            Value = value;
        }

        internal static VirtualAccountId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        internal static VirtualAccountId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}