namespace MoneyPipe.API.DTOs.Responses
{
    public record GetWalletDTO(string Currency,decimal Balance,Guid Id);
}