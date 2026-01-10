namespace MoneyPipe.API.DTOs.Responses
{
    public record GetWalletDTO(string Currency,decimal AvailableBalance,decimal PendingBalance,Guid Id);
}