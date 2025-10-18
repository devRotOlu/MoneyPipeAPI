namespace MoneyPipe.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Currency { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
