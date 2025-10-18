namespace MoneyPipe.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string Type { get; set; } = null!; // "Credit" or "Debit"
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
