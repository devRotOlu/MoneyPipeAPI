using MoneyPipe.Application.Enum;
using MoneyPipe.Application.Interfaces.IServices;

namespace MoneyPipe.Infrastructure.Services
{
    public class FlutterwaveVirtualAccountProcessor(IVirtualAccountProvisionerFactory virtualAccountProvisionerFactory) 
    : VirtualAccountProcessor
    {
        public override VirtualAccountMethod Method => VirtualAccountMethod.FlutterWave;
        private readonly IVirtualAccountProvisionerFactory _virtualAccountProvisionerFactory = virtualAccountProvisionerFactory;

        public override IVirtualAccountProvisioner CreateVirtualAccount()
        {
            return _virtualAccountProvisionerFactory.Create(Method);
        }
    }
}