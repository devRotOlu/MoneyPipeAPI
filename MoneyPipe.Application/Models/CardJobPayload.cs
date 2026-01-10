using System.Text.Json;
using MoneyPipe.Application.TypeJsonConverters;
using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Models
{
    public record CardJobPayload
    {
        public CardJobPayload(UserId userId,WalletId walletId,string currency)
        {
            UserId = userId;
            WalletId = walletId;
            Currency = currency;
        }

        public UserId UserId {get;} 
        public WalletId WalletId {get;} 
        public string Currency {get;}

        public JsonDocument Serialize()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new UserIdConverter());
            options.Converters.Add(new WalletIdConverter());
            return JsonDocument.Parse(JsonSerializer.Serialize(this,options));
        }

         public static CardJobPayload? Deserialize(JsonDocument payload)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new UserIdConverter());
            options.Converters.Add(new WalletIdConverter());
            return JsonSerializer.Deserialize<CardJobPayload>(payload,options);
        }
    }
}