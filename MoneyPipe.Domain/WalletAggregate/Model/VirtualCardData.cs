namespace MoneyPipe.Domain.WalletAggregate.Model
{
    public record VirtualCardData(string CardNumber,int ExpiryMonth,
    int ExpiryYear, string CVC,decimal? Limit,string Currency);
}