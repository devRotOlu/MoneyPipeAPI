namespace MoneyPipe.Application.DTOs
{
    public record RefreshRequestDTO
    {
        public string RefreshToken { get; set; } = null!;
    }
}
