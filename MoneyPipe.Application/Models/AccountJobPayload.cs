using System.Text.Json;
using MoneyPipe.Application.TypeJsonConverters;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Models
{
    public record AccountJobPayload
    {
        public AccountJobPayload(string userEmail,string currency,WalletId walletId)
        {
            UserEmail = userEmail;
            Currency = currency;
            WalletId = walletId;
        }

        public string UserEmail {get;}
        public string Currency {get;}
        public WalletId WalletId {get;}

        public JsonDocument Serialize()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new WalletIdConverter());
            return JsonDocument.Parse(JsonSerializer.Serialize(this,options));
        }

        public static AccountJobPayload? Deserialize(JsonDocument payload)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new WalletIdConverter());
            return JsonSerializer.Deserialize<AccountJobPayload>(payload,options);
        }
    };
}