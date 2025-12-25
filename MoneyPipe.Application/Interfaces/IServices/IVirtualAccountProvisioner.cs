using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IVirtualAccountProvisioner
    {
        Task<VirtualAccountResponse> CreateAsync(VirtualAccountId virtualAccountId, string email);
    }
}