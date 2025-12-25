using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Models
{
    public record AccountJobPayload
    {
        public AccountJobPayload(string userEmail,string currency,WalletId walletId)
        {
            UserEmail = userEmail;
            Currency = currency;
            WalletId = walletId;
        }

        public string UserEmail {get;}
        public string Currency {get;}
        public WalletId WalletId {get;}
    };
}