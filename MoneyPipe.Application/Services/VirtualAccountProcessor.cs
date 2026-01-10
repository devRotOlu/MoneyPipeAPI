using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Services
{
    public abstract class VirtualAccountProcessor:IVirtualAccountProcessor
    {

        public abstract VirtualAccountMethod Method {get;}

        public async Task<VirtualAccountResponse> ProcessVirtualAccount(VirtualAccountId virtualAccountId, string email)
        {
            var accountProvisioner = CreateVirtualAccount();
            var response = await accountProvisioner.CreateVirtualAccountAsync(virtualAccountId, email);
            return response;
        } 

        public abstract IVirtualAccountProvisioner CreateVirtualAccount();

    }
}