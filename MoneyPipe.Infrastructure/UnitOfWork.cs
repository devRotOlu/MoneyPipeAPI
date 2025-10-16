using MoneyPipe.Application.Interfaces;
using System.Data;


namespace MoneyPipe.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        //public IMovieRepository Movies { get; }

        private IDbTransaction _transaction;

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _transaction = _dbConnection.BeginTransaction();
            //Movies = new MovieRepository(_dbConnection, _transaction);
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
