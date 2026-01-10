namespace MoneyPipe.Infrastructure.Services.Models.Response
{
    public class PaystackVirtualAccountResponse
    {
        public ResponseData Data { get; init; } = null!;

        public record ResponseData(string AccountId, string AccountNumber, string BankName, string Currency);

    }

}