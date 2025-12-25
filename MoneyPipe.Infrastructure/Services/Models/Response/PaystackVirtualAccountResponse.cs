namespace MoneyPipe.Infrastructure.Services.Models.Response
{
    public class PaystackVirtualAccountResponse
    {
        public Data Data { get; init; } = null!;
    }

    public record Data(string AccountId, string AccountNumber, string BankName, string Currency);

}