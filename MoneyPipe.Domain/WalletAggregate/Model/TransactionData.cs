using MoneyPipe.Domain.WalletAggregate.Enums;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Domain.WalletAggregate.Model
{
    public record TransactionData
    {
        public decimal Amount {get;set;}
        public string Currency {get;set;} = null!;
        public TransactionDirection Direction {get;set;}
        public TransactionType Type {get;set;}
        public string Provider {get;set;} = null!;
        public string ProviderReference {get;set;} = null!;
        public TransactionId? RelatedTransactionId {get;set;}
        public TransactionStatus Status {get;set;}
    }
}