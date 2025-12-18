using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate;

namespace MoneyPipe.Domain.Services
{
    public static class WalletCreationService
    {
        public static ErrorOr<Wallet> CreateWallet(IEnumerable<Wallet> previousWallets,UserId userId,string currency)
        {
            var walletExits = previousWallets.Any((wallet) =>wallet.Currency == currency);

            if (walletExits) return Errors.Wallet.AlreadyExists;

            var result = Wallet.Create(userId,currency);

            if (result.IsError) return result.Errors;

            return result.Value;
        }
    }
}