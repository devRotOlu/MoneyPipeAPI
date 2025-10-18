namespace MoneyPipe.Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string? Description { get; set; }
        public string? PaymentUrl { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
