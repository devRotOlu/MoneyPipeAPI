using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IRepository;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Infrastructure.Persistence.Repositories.Writes;
using MoneyPipe.Infrastructure.Repository;
using System.Data;


namespace MoneyPipe.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _transaction;

        public IUserWriteRepository Users { get; set; }
        // public IRefreshTokenRepository RefreshTokens { get; set; }
        // public IPasswordResetRepository PasswordRestTokens { get; set; }
        // public IInvoiceRepository Invoices { get; set; }

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _transaction = _dbConnection.BeginTransaction();
            Users = new UserWriteRepository(dbConnection,_transaction);
            // RefreshTokens = new RefreshTokenRepository(dbConnection, _transaction);
            // PasswordRestTokens = new PasswordResetRepository(dbConnection, _transaction);
            // Invoices = new InvoiceRepository(dbConnection, _transaction);
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
