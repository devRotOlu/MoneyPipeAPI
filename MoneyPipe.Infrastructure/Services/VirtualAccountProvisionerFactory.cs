using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Enum;
using MoneyPipe.Application.Interfaces.IServices;

namespace MoneyPipe.Infrastructure.Services
{
    public class VirtualAccountProvisionerFactory(IServiceScopeFactory scopeFactory) : IVirtualAccountProvisionerFactory
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public IVirtualAccountProvisioner Create(VirtualAccountMethod method)
        {
            using var scope = _scopeFactory.CreateScope();
            return method switch
            {
                VirtualAccountMethod.FlutterWave => scope.ServiceProvider.GetRequiredService<FlutterWaveVirtualAccountProvisioner>(),
                VirtualAccountMethod.Paystack => scope.ServiceProvider.GetRequiredService<PaystackVirtualAccountProvisioner>(),
                _ => throw new Exception()
            };
        }
    }
}