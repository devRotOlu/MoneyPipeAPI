using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Domain.Common.Interfaces;

namespace MoneyPipe.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserWriteRepository Users { get; set; }
        IInvoiceWriteRepository Invoices {get;set;}
        IEmailJobWriteRepository EmailJobs {get;set;}
        IBackgroundJobWriteRepository BackgroundJobs {get;set;}
        IWalletWriteRepository Wallets {get;set;}
        Task Commit();
        void Rollback();
        Task RegisterAggregateAsync(IAggregateRoot aggregate);
    }
}
