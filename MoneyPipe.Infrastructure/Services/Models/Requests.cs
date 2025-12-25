namespace MoneyPipe.Infrastructure.Services.Models
{
    public sealed class PaystackOptions
    {
        public string BaseUrl { get; init; } = "https://api.paystack.co";
        public string SecretKey { get; init; } // Your secret key
    }
}