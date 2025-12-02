namespace MoneyPipe.Domain.Models
{
    public record EditInvoiceItemRequest:InvoiceItemRequest
    {
        public Guid Id { get; set; }

        public string? InvoiceItemId { get; set; } = null!; 
    }
}