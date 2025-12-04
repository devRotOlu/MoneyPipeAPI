using MoneyPipe.Domain.InvoiceAggregate.Models;

namespace MoneyPipe.Domain.InvoiceAggregate.Interfaces
{
    public interface IInvoiceData
    {
        IEnumerable<InvoiceItemData> InvoiceItems { get; }
    }
}