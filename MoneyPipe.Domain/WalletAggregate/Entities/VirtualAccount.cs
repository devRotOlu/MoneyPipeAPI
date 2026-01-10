using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.WalletAggregate.Model;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Domain.WalletAggregate.Entities
{
    public class VirtualAccount:Entity<VirtualAccountId>
    {
        public WalletId WalletId { get; private set; } = null!;
        public string BankName { get; private set;}
        public string AccountName { get; private set;}
        public string ProviderName {get;private set;}
        public string ProviderAccountId { get; private set; }
        public bool IsActive {get; private set;} =  true;
        public DateTime CreatedAt {get; private set;} = DateTime.UtcNow;
        public DateTime UpdatedAt {get; private set;} = DateTime.UtcNow;
        public string Currency {get;private set;}
        public bool IsPrimaryForInvoice {get; private set;} 

        private VirtualAccount(VirtualAccountId id):base(id)
        {
            
        }

        private VirtualAccount()
        {
            
        }

        public static ErrorOr<VirtualAccount> Create(VirtualAccountData data,WalletId walletId,
        VirtualAccountId id, bool isPrimaryAccount)
        {
            List<Error> errors = [];

            if (string.IsNullOrEmpty(data.BankName)) errors.Add(Errors.VirtualAccount.MissingBankNameError);
            if (string.IsNullOrEmpty(data.AccountName)) errors.Add(Errors.VirtualAccount.MissingAccountNameError);
            if (string.IsNullOrEmpty(data.Currency)) errors.Add(Errors.VirtualAccount.MissingCurrencyError);

            if (errors.Count != 0) return errors;

            return new VirtualAccount(id)
            {
                WalletId = walletId,
                BankName = data.BankName,
                AccountName = data.AccountName,
                Currency = data.Currency,
                ProviderAccountId = data.ProviderAccountId,
                ProviderName = data.ProviderName,
                IsPrimaryForInvoice = isPrimaryAccount
            };
        } 

        internal void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void ChangeAsPrimaryAccount()=> IsPrimaryForInvoice = !IsPrimaryForInvoice;
    }
}