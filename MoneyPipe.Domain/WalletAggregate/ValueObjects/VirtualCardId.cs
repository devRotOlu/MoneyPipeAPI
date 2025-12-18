using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.WalletAggregate.ValueObjects
{
    public sealed class VirtualCardId:ValueObject
    {
        public Guid Value { get;}

        private VirtualCardId(Guid value)
        {
            Value = value;
        }

        internal static VirtualCardId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        internal static VirtualCardId CreateUnique(Guid id)
        {
            return new(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}