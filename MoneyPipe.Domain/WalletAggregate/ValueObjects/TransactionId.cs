using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.WalletAggregate.ValueObjects
{
    public sealed class TransactionId : BaseId<TransactionId>
    {
        public TransactionId()
        {
        }
    }
}