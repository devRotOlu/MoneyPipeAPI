using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Services;

namespace MoneyPipe.Infrastructure.Services.PaymentCreation
{
    public class FlutterWavePaymentCreationProcessor(IPaymentCreationProvisonerFactory paymentCreationProvisonerFactory) : PaymentCreationProcessor
    {
        public override PaymentCreationMethod Method => PaymentCreationMethod.FlutterWave;
        private readonly IPaymentCreationProvisonerFactory _paymentCreationProvisonerFactory = paymentCreationProvisonerFactory;

        public override IPaymentCreationProvisioner CreatePayment()
        {
            return _paymentCreationProvisonerFactory.Create(Method);
        }
    }
}