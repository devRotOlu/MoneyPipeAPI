using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Services;

namespace MoneyPipe.Infrastructure.Services.VirtualAccount
{
    public class MonnifyVirtualAccountProcessor(IVirtualAccountProvisionerFactory virtualAccountProvisionerFactory) 
    : VirtualAccountProcessor
    {
        public override VirtualAccountMethod Method => VirtualAccountMethod.Monnify;
        private readonly IVirtualAccountProvisionerFactory _virtualAccountProvisionerFactory = virtualAccountProvisionerFactory;

        public override IVirtualAccountProvisioner CreateVirtualAccount()
        {
            return _virtualAccountProvisionerFactory.Create(Method);
        }
    }
}