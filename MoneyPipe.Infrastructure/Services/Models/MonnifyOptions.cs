namespace MoneyPipe.Infrastructure.Services.Models
{
    public sealed class MonnifyOptions
    {
        public string BaseUrl { get; set; } = "https://sandbox.monnify.com";
        public string ApiKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string ContractCode { get; set; } = default!;
    }
}