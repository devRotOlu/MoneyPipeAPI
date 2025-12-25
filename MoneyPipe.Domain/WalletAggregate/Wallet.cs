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
        public decimal Balance { get; private set; } = 0;
        public bool IsActive {get; private set;} = true;
        public DateTime CreatedAt { get;private set; } =  DateTime.UtcNow;
        public DateTime UpdatedAt { get;private set; } =  DateTime.UtcNow;
        private readonly List<VirtualAccount> _virtualAccounts = [];
        private readonly List<VirtualCard> _virtualCards = [];
        public IReadOnlyList<VirtualAccount> VirtualAccounts => _virtualAccounts.AsReadOnly();
        public IReadOnlyList<VirtualCard> VirtualCards => _virtualCards.AsReadOnly();

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
            Balance += amount;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success;
        }

        public ErrorOr<Success> Debit(decimal amount)
        {
            if (amount > Balance) return Errors.Wallet.InsufficientFundsError;
            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success;
        }

        public ErrorOr<Success> AddVirtualAccount(VirtualAccountData data)
        {
            var result = VirtualAccount.Create(data,Id);
            if (result.IsError) return result.Errors;

            _virtualAccounts.Add(result.Value);
            return Result.Success;
        }

        public ErrorOr<Success> AddVirtualCard(VirtualCardData data)
        {
            // card limit must not be more than its wallet balance
            if (data.Limit > Balance) return Errors.Wallet.VirtualCardLimitExceededError;

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
    }
}