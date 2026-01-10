using ErrorOr;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.WalletAggregate.Enums;
using MoneyPipe.Domain.WalletAggregate.Model;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Domain.WalletAggregate.Entities
{
    public class Transaction:Entity<TransactionId>
    {
        private Transaction(TransactionId id):base(id)
        {
            
        }

        private Transaction()
        {
            
        }


        public WalletId WalletId { get; private set; } = null!;
        public string Direction { get; private set; } = null!; // "Credit" or "Debit"
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public string Status {get;private set;} = null!; 
        public string Type {get;private set;} = null!; 
        public string ProviderName {get; private set;} = null!; // provider reference
        public string ProviderReference {get; private set;} = null!;
        public bool IsVerified {get; private set;} = false;
        public TransactionId? RelatedTransactionId {get; private set;} 

        internal static ErrorOr<Transaction> Create(TransactionData data,TransactionId id)
        {
            return new Transaction(id)
            {
                Amount = data.Amount,
                Direction = data.Direction.ToString(),
                Currency = data.Currency,
                Status = data.Status.ToString(),
                Type = data.Type.ToString(),
                ProviderName = data.Provider,
                ProviderReference = data.ProviderReference,
                RelatedTransactionId = data.RelatedTransactionId
            };
        }

        internal void ChangeStatus(TransactionStatus status)
        {
            Status = status.ToString();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}