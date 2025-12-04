
namespace MoneyPipe.Domain.InvoiceAggregate.Models
{
    public record InvoiceData
    {
        public string Currency { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime? DueDate { get; set; }
        public string CustomerEmail { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string? CustomerAddress { get; private set; }
        public IEnumerable<InvoiceItemData> InvoiceItems { get; set; } = null!;
    }
}