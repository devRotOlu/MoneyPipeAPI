namespace MoneyPipe.API.DTOs
{
    public class GetInvoiceDTO
    {
        public string Id { get; set; } = null!;
        public string InvoiceNumber { get; set; } = null!;
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set;} = null!;
        public string Status { get; set;} = "Draft";
        public DateTime? DueDate { get; set;}
        public DateTime IssueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;
        public string? CustomerAddress { get; set; } 
        public string? Notes { get; set; }
        public string? PaymentUrl { get; set; }
        public IEnumerable<GetInvoiceItemDTO> InvoiceItems { get; set; } = null!;
    }
}