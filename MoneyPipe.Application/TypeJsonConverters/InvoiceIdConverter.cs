using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Application.TypeJsonConverters
{
     public sealed class InvoiceIdConverter : JsonConverter<InvoiceId>
    {
        public override InvoiceId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return InvoiceId.CreateUnique(Guid.Parse(value!)).Value; // or however you construct InvoiceId
        }

        public override void Write(Utf8JsonWriter writer, InvoiceId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value.ToString());
        }
    }
}