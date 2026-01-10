using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IVirtualAccountProvisioner
    {
        Task<VirtualAccountResponse> CreateVirtualAccountAsync(VirtualAccountId virtualAccountId, string email);
    }
}