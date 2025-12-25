using MoneyPipe.Application.Enum;
using MoneyPipe.Application.Interfaces.IServices;


namespace MoneyPipe.Infrastructure.Services
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