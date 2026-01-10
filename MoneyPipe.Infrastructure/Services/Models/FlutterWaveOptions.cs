namespace MoneyPipe.Infrastructure.Services.Models
{
    public sealed class FlutterWaveOptions
    {
        public string BaseUrl { get; init; } = "https://developersandbox-api.flutterwave.com";

        public string ApiSecret {get;init;} = null!;
    }
}