using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Application.Interfaces
{
    public interface IBackgroundJobQueue
    {
        Task EnqueueSendInvoiceAsync(InvoiceId invoiceId);
    }
}