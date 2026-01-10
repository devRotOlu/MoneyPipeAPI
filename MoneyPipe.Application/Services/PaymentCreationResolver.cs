using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;

namespace MoneyPipe.Application.Services
{
    public sealed class PaymentCreationResolver
    {
        public PaymentCreationResolver(IEnumerable<IPaymentCreationProcessor> processors)
        {
            Processors = processors.ToDictionary(p => p.Method);
        }

        private Dictionary<PaymentCreationMethod,IPaymentCreationProcessor> Processors {get;}

        public IPaymentCreationProcessor Resolve()
        {
            return Processors[PaymentCreationMethod.FlutterWave];
        }

    }
}