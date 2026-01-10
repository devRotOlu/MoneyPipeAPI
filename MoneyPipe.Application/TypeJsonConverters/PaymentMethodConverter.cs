using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyPipe.Domain.InvoiceAggregate.Enums;

namespace MoneyPipe.Application.TypeJsonConverters
{
    public sealed class PaymentMethodConverter : JsonConverter<PaymentMethod>
    {
        public override PaymentMethod Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return Enum.Parse<PaymentMethod>(value!,true);
        }

        public override void Write(Utf8JsonWriter writer, PaymentMethod value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}