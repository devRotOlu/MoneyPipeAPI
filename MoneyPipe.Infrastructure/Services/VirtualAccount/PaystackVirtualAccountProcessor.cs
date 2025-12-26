using MoneyPipe.Application.Enum;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Services;


namespace MoneyPipe.Infrastructure.Services.VirtualAccount
{
    public class PaystackVirtualAccountProcessor(IVirtualAccountProvisionerFactory virtualAccountProvisionerFactory) 
    : VirtualAccountProcessor
    {
        private readonly IVirtualAccountProvisionerFactory _virtualAccountProvisionerFactory = virtualAccountProvisionerFactory;
        public override VirtualAccountMethod Method => VirtualAccountMethod.Paystack;

        public override IVirtualAccountProvisioner CreateVirtualAccount()
        {
            return _virtualAccountProvisionerFactory.Create(Method);
        }
    }
}