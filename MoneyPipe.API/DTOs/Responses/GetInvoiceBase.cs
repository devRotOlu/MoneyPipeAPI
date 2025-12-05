namespace MoneyPipe.API.DTOs.Responses
{
    public record GetInvoiceBase
    {
        public string Id { get; init; } = null!;
        public string InvoiceNumber { get; init; } = null!;
        public decimal SubTotal { get; init; }
        public decimal TaxAmount { get; init; }
        public decimal TotalAmount { get; init; }
        public string Currency { get; init;} = null!;
        public string Status { get; init;} = "Draft";
        public DateTime? DueDate { get; init;}
        public DateTime IssueDate { get; init; }
        public DateTime? PaidAt { get; init; }
        public string CustomerName { get; init; } = null!;
        public string CustomerEmail { get; init; } = null!;
        public string? CustomerAddress { get; init; } 
        public string? Notes { get; init; }
        public string? PaymentUrl { get; init; }
    }
}