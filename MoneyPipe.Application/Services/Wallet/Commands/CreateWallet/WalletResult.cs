namespace MoneyPipe.Application.Services.Wallet.Commands.CreateWallet
{
    public record WalletResult(string Currency,decimal Balance,Guid Id);
}