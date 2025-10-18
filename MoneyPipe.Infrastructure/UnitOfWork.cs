using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IRepository;
using MoneyPipe.Infrastructure.Repository;
using System.Data;


namespace MoneyPipe.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction _transaction;

        public IUserRepository Users { get; set; }
        public IRefreshTokenRepository RefreshTokens { get; set; }

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _transaction = _dbConnection.BeginTransaction();
            Users = new UserRepository(dbConnection,_transaction);
            RefreshTokens = new RefreshTokenRepository(dbConnection,_transaction);
        }

        public void CommitAsync()
        {
            _transaction.Commit();
            _dbConnection.Close();
        }

        public void RollbackAsync()
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
