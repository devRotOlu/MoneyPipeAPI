using MoneyPipe.Domain.InvoiceAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Reads
{
    public interface IInvoiceReadRepository
    {
        Task<Invoice?> GetByIdAsync(Guid invoiceId);
        Task<int> GetNextInvoiceNumberAsync();
        Task<IEnumerable<Invoice>> GetInvoicesAsync(Guid userId,int pageSize,DateTime? lastTimestamp);
    }
}