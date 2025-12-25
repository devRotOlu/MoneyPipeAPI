using MoneyPipe.Application.Enum;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IVirtualAccountProcessor
    {
        VirtualAccountMethod Method {get;}
        Task<VirtualAccountResponse> ProcessVirtualAccount(VirtualAccountId virtualAccountId, string email);
    }
}