using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;

namespace MoneyPipe.Infrastructure.Services.PaymentCreation
{
    public class PaymentCreationProvisionerFactory(IServiceScopeFactory scopeFactory) : IPaymentCreationProvisonerFactory
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        public IPaymentCreationProvisioner Create(PaymentCreationMethod method)
        {
            using var scope = _scopeFactory.CreateScope();
            return method switch
            {
                PaymentCreationMethod.FlutterWave => scope.ServiceProvider.GetRequiredService<FlutterWave>(),
                _ => throw new Exception()
            };
        }
    }
}