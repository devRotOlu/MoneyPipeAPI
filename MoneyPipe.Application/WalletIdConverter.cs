using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application
{
    public sealed class WalletIdConverter : JsonConverter<WalletId>
    {
        public override WalletId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return WalletId.CreateUnique(Guid.Parse(value!)).Value; // or however you construct WalletId
        }

        public override void Write(Utf8JsonWriter writer, WalletId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value.ToString());
        }
    }

}