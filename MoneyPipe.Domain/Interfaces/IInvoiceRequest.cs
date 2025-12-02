using MoneyPipe.Domain.Models;

namespace MoneyPipe.Domain.Interfaces
{
    public interface IInvoiceRequest
    {
        IEnumerable<InvoiceItemRequest> InvoiceItems { get; }
    }
}