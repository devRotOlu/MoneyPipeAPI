namespace MoneyPipe.Application.ReadModels
{
    public record PasswordResetTokenDTO(string Token,DateTime ExpiresAt,bool IsUsed,DateTime CreatedAt);
}