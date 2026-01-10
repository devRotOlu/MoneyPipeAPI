using MoneyPipe.Application.Enums;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IPaymentCreationProvisonerFactory
    {
        IPaymentCreationProvisioner Create(PaymentCreationMethod method);
    }
}