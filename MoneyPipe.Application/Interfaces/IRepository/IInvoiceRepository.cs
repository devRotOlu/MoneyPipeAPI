using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Interfaces.IRepository
{
        public interface IInvoiceRepository:IGenericRepository<Invoice>
    {
        Task<int> GetNextInvoiceNumberAsync();
        Task SaveInvoiceAsync(Invoice invoice);
        Task<Invoice?> GetInvoiceByIdAsync(Guid id);
        Task<IEnumerable<Invoice>> GetInvoicesByUserAsync(Guid userId);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task EditInvoiceAsync(Invoice invoice);
    }
}