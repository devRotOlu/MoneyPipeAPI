using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Models;

namespace MoneyPipe.Application.Interfaces.IServices
{
   public interface IPaymentCreationProcessor
   {
     PaymentCreationMethod Method {get;}
     Task<PaymentCreationResponse> ProcessPaymentCreation(string paymentRef, decimal amount,string email,
     string currency, CancellationToken ct);   
   }
}