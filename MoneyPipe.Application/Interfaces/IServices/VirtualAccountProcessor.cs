using MoneyPipe.Application.Enum;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public abstract class VirtualAccountProcessor:IVirtualAccountProcessor
    {

        public abstract VirtualAccountMethod Method {get;}

        public async Task<VirtualAccountResponse> ProcessVirtualAccount(VirtualAccountId virtualAccountId, string email)
        {
            var accountProvisioner = CreateVirtualAccount();
            var response = await accountProvisioner.CreateAsync(virtualAccountId, email);
            return response;
        } 

        public abstract IVirtualAccountProvisioner CreateVirtualAccount();

    }
}