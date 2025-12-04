using MoneyPipe.Domain.InvoiceAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Writes
{
    public interface IInvoiceWriteRepository
    {
        Task InsertAsync(Invoice invoice);
        Task UpdateAsync(Invoice invoice);
        Task DeleteAsync(Invoice invoice);
    }
}