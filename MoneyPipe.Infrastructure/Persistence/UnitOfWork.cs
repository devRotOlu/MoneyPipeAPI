using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Infrastructure.Persistence.Repositories.Writes;
using System.Data;


namespace MoneyPipe.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _transaction;

        public IUserWriteRepository Users { get; set; }
        public IInvoiceWriteRepository Invoices {get;set;}
        public IEmailJobWriteRepository EmailJobs { get; set; }
        public IBackgroundJobWriteRepository BackgroundJobs { get; set; }

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _transaction = _dbConnection.BeginTransaction();
            Users = new UserWriteRepository(dbConnection,_transaction);
            Invoices = new InvoiceWriteRepository(dbConnection,_transaction);
            EmailJobs = new EmailJobWriteRepository(dbConnection,_transaction);
            BackgroundJobs = new BackgroundJobWriteRepository(dbConnection,_transaction);
        }

        public void Commit()
        {
            _transaction.Commit();
            _dbConnection.Close();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _dbConnection.Close();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbConnection?.Dispose();
        }
    }
}
