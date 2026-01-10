namespace MoneyPipe.Infrastructure.Services.Models
{
    public class KorapayOptions
    {
        public string BaseUrl { get; set; } = "https://api.korapay.com";
        public string SecretKey {get;set;} = null!;
    }
}