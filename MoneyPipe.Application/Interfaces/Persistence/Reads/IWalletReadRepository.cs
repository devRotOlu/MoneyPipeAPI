using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Reads
{
    public interface IWalletReadRepository
    {
        Task<IEnumerable<Wallet>> GetWallets(UserId userid);
    }
}