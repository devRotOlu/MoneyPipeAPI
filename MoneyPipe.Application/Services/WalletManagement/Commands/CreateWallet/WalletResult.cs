namespace MoneyPipe.Application.Services.WalletManagement.Commands.CreateWallet
{
    public record WalletResult
    {
        public string Currency {get;init;} = null!;
        public decimal AvailableBalance {get;init;}
        public decimal PendingBalance {get;init;}
        public Guid Id {get;init;}
    };
}