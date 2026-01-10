using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;

namespace MoneyPipe.Application.Services
{
    public abstract class PaymentCreationProcessor : IPaymentCreationProcessor
    {
        public abstract PaymentCreationMethod Method {get;}

        public Task<PaymentCreationResponse> ProcessPaymentCreation(string paymentRef, decimal amount, 
        string email, string currency,CancellationToken ct)
        {
            var paymentProvisioner = CreatePayment();
            var response = paymentProvisioner.CreatePayment(paymentRef,amount,email,currency,ct);
            return response;
        }

        public abstract IPaymentCreationProvisioner CreatePayment();
    }
}