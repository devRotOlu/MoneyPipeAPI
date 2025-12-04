using MoneyPipe.Domain.InvoiceAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Reads
{
    public interface IInvoiceReadRepository
    {
        Task<Invoice?> GetByIdAsync(Guid invoiceId);
        Task<int> GetNextInvoiceNumberAsync();
    }
}