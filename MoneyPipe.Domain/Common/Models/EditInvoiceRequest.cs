using MoneyPipe.Domain.Interfaces;

namespace MoneyPipe.Domain.Models
{
    public record EditInvoiceRequest:InvoiceRequest,IInvoiceRequest
    {
        public Guid Id { get; set; }
        public new IEnumerable<EditInvoiceItemRequest> InvoiceItems { get; set; } = null!;
        IEnumerable<InvoiceItemRequest> IInvoiceRequest.InvoiceItems => InvoiceItems;
    }
}