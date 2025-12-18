namespace MoneyPipe.Domain.WalletAggregate.Model
{
    public record VirtualCardData(string CardNumber,DateOnly ExpiryDate,
    string CVC,decimal? Limit,string Currency);
}