using MoneyPipe.Domain.WalletAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Writes
{
    public interface IWalletWriteRepository
    {
        Task CreateWallet(Wallet wallet);
        Task AddVirtualAccount(Wallet wallet);
    }
}