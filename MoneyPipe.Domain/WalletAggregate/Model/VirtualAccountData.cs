using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Domain.WalletAggregate.Model
{
    public record VirtualAccountData(string BankName,string AccountName,
    string Currency,string ProviderName,string ProviderAccountId,VirtualAccountId Id);
}