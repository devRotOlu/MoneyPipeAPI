using System.Text.Json;
using System.Text.Json.Serialization;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Application.TypeJsonConverters
{
    public sealed class UserIdConverter : JsonConverter<UserId>
    {
        public override UserId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return UserId.CreateUnique(Guid.Parse(value!)).Value; // or however you construct UserId
        }

        public override void Write(Utf8JsonWriter writer, UserId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value.ToString());
        }
    }
}