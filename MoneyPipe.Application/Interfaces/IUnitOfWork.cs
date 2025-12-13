using MoneyPipe.Application.Interfaces.Persistence.Writes;

namespace MoneyPipe.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserWriteRepository Users { get; set; }
        IInvoiceWriteRepository Invoices {get;set;}
        IEmailJobWriteRepository EmailJobs {get;set;}
        IBackgroundJobWriteRepository BackgroundJobs {get;set;}
        void Commit();
        void Rollback();
    }
}
