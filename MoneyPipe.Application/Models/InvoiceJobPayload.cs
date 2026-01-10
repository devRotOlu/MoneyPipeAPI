using System.Text.Json;
using MoneyPipe.Application.TypeJsonConverters;
using MoneyPipe.Domain.InvoiceAggregate.Enums;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Models
{
    public record InvoiceJobPayload
    {
        public InvoiceJobPayload(InvoiceId invoiceId,WalletId walletId,PaymentMethod paymentMethod)
        {
            InvoiceId = invoiceId;
            WalletId = walletId;
            PaymentMethod = paymentMethod;
        }

        public InvoiceId InvoiceId {get;}
        public WalletId WalletId {get;}
        public PaymentMethod PaymentMethod {get;}

        public JsonDocument Serialize()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new WalletIdConverter());
            options.Converters.Add(new InvoiceIdConverter());
            options.Converters.Add(new PaymentMethodConverter());
            return JsonDocument.Parse(JsonSerializer.Serialize(this,options));
        }

        public static InvoiceJobPayload? Deserialize(JsonDocument payload)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new WalletIdConverter());
            options.Converters.Add(new InvoiceIdConverter());
            options.Converters.Add(new PaymentMethodConverter());
            return JsonSerializer.Deserialize<InvoiceJobPayload>(payload,options);
        }
    }
}