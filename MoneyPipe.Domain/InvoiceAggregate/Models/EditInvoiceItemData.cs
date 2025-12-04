namespace MoneyPipe.Domain.InvoiceAggregate.Models
{
    public record EditInvoiceItemData:InvoiceItemData
    {
        public Guid Id { get; set; }

        public string? InvoiceItemId { get; set; } = null!; 
    }
}