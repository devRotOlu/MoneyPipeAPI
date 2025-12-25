using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Domain.Services
{
    public static class WalletCreationService
    {
        public static ErrorOr<Wallet> CreateWallet(IEnumerable<Wallet> previousWallets,UserId userId,string currency)
        {
            var walletExits = previousWallets.Any((wallet) =>wallet.Currency == currency);

            if (walletExits) return Errors.Wallet.AlreadyExists;

            var idResult = WalletId.CreateUnique(Guid.NewGuid());

            var result = Wallet.Create(idResult.Value,userId,currency);

            if (result.IsError) return result.Errors;

            return result.Value;
        }
    }
}