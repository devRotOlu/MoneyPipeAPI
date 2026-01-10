using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate.Entities;
using MoneyPipe.Domain.WalletAggregate.Model;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Domain.WalletAggregate
{
    public class Wallet:AggregateRoot<WalletId>
    {
        private Wallet(WalletId id):base(id)
        {
            
        }

        private Wallet()
        {
            
        }

        public UserId UserId { get; private set; }
        public string Currency { get; private set; } = null!;
        public decimal AvailableBalance { get; private set; } = 0;
        public decimal PendingBalance {get;private set;} = 0;
        public bool IsActive {get; private set;} = true;
        public DateTime CreatedAt { get;private set; } =  DateTime.UtcNow;
        public DateTime UpdatedAt { get;private set; } =  DateTime.UtcNow;
        private readonly List<VirtualAccount> _virtualAccounts = [];
        private readonly List<VirtualCard> _virtualCards = [];
        private readonly List<Transaction> _transactions = [];
        public IReadOnlyList<VirtualAccount> VirtualAccounts => _virtualAccounts.AsReadOnly();
        public IReadOnlyList<VirtualCard> VirtualCards => _virtualCards.AsReadOnly();
        public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

        internal static ErrorOr<Wallet> Create(WalletId walletId, UserId userId,string currency)
        {
            if (string.IsNullOrEmpty(currency)) return Errors.Wallet.MissingCurrencyError;

            return new Wallet(walletId)
            {
                UserId = userId,
                Currency = currency
            };
        }

        public ErrorOr<Success> Credit(decimal amount)
        {
            if (amount <= 0) return Errors.Wallet.InvalidCreditAmountError;
            AvailableBalance += amount;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success;
        }

        public ErrorOr<Success> Debit(decimal amount)
        {
            if (amount > AvailableBalance) return Errors.Wallet.InsufficientFundsError;
            AvailableBalance -= amount;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success;
        }

        public ErrorOr<Success> AddVirtualAccount(VirtualAccountData data,VirtualAccountId id)
        {
            var isPrimaryAccount = _virtualAccounts.Any(account=>!account.IsPrimaryForInvoice); 
            var result = VirtualAccount.Create(data,Id,id,isPrimaryAccount);
            if (result.IsError) return result.Errors;

            _virtualAccounts.Add(result.Value);
            return Result.Success;
        }

        public void ChangePrimaryAccount(VirtualAccountId newPrimaryAccountId)
        {
            var oldPrimaryAccount = _virtualAccounts.Find(account=> account.IsPrimaryForInvoice);
            oldPrimaryAccount!.ChangeAsPrimaryAccount();
            var newPrimaryAccount = _virtualAccounts.Find(account=> account.Id == newPrimaryAccountId);
            newPrimaryAccount?.ChangeAsPrimaryAccount();
        }

        public ErrorOr<Success> AddVirtualCard(VirtualCardData data)
        {
            // card limit must not be more than its wallet balance
            if (data.Limit > AvailableBalance) return Errors.Wallet.VirtualCardLimitExceededError;

            var result = VirtualCard.Create(data,Id);

            if (result.IsError) return result.Errors;

            _virtualCards.Add(result.Value);

            return Result.Success;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Infrastructure-only method for rehydrating invoice items from persistence.
        /// Do not use in business logic.
        /// </summary>
        public void AddVirtualAccounts(IEnumerable<VirtualAccount> virtualAccounts)=> _virtualAccounts.AddRange(virtualAccounts);

        /// <summary>
        /// Infrastructure-only method for rehydrating invoice items from persistence.
        /// Do not use in business logic.
        /// </summary>
        public void AddVirtualCards(IEnumerable<VirtualCard> virtualCards)=> _virtualCards.AddRange(virtualCards);
    }
}