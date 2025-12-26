using MoneyPipe.Application.Enum;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Services;

namespace MoneyPipe.Infrastructure.Services.VirtualAccount
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