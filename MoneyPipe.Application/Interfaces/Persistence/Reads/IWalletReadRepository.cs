using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Interfaces.Persistence.Reads
{
    public interface IWalletReadRepository
    {
        Task<IEnumerable<Wallet>> GetWallets(UserId userid);
        Task<Wallet?> GetWallet(WalletId walletId);
    }
}