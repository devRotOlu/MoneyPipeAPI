namespace MoneyPipe.Application.Models
{
    public record VirtualAccountResponse(string AccountId, string AccountNumber, string BankName, 
    string Currency, string ProviderName);
}