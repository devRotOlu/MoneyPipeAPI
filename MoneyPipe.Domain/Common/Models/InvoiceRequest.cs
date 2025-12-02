
using MoneyPipe.Domain.Interfaces;

namespace MoneyPipe.Domain.Models
{
        public record InvoiceRequest:IInvoiceRequest
    {
        public string Currency { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime? DueDate { get; set; }
        public string CustomerEmail { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string? CustomerAddress { get; private set; }
        public IEnumerable<InvoiceItemRequest> InvoiceItems { get; set; } = null!;
    }
}