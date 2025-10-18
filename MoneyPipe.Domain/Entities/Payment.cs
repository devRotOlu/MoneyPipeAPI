namespace MoneyPipe.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public string Reference { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime? PaidAt { get; set; }
        public string Gateway { get; set; } = "Paystack";
        public string? RawResponse { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
