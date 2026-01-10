using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;

namespace MoneyPipe.Infrastructure.Services.VirtualAccount
{
    public class VirtualAccountProvisionerFactory(IServiceScopeFactory scopeFactory) : IVirtualAccountProvisionerFactory
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public IVirtualAccountProvisioner Create(VirtualAccountMethod method)
        {
            using var scope = _scopeFactory.CreateScope();
            return method switch
            {
                VirtualAccountMethod.FlutterWave => scope.ServiceProvider.GetRequiredService<FlutterWave>(),
                VirtualAccountMethod.Paystack => scope.ServiceProvider.GetRequiredService<Paystack>(),
                VirtualAccountMethod.Monnify => scope.ServiceProvider.GetRequiredService<Monnify>(),
                VirtualAccountMethod.Korapay => scope.ServiceProvider.GetRequiredService<Korapay>(),
                _ => throw new Exception()
            };
        }
    }
}