using MoneyPipe.Application.Models;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IPaymentCreationProvisioner
    {
        Task<PaymentCreationResponse> CreatePayment(string paymentRef, decimal amount,string email,
     string currency,CancellationToken ct);
    }
}