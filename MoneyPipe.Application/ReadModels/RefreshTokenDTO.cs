namespace MoneyPipe.Application.ReadModels
{
    public record RefreshTokenDTO(DateTime ExpiresAt,DateTime RevokedAt,DateTime CreatedAt,string Token);
}