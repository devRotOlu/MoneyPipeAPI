using Dapper.Contrib.Extensions;
using MoneyPipe.Application.Interfaces.IRepository;
using System.Data;

namespace MoneyPipe.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection _dbConnection;
        protected readonly IDbTransaction _transaction;

        public GenericRepository(IDbConnection dbConnection, IDbTransaction transaction)
        {
            _dbConnection = dbConnection;
            _transaction = transaction;
        }
        public async Task<int> AddAsync(T entity)
        {
            return await _dbConnection.InsertAsync(entity, _transaction);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbConnection.GetAsync<T>(id);
            if (entity == null) return false;
            return await _dbConnection.DeleteAsync(entity, _transaction);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbConnection.GetAllAsync<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbConnection.GetAsync<T>(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            return await _dbConnection.UpdateAsync(entity, _transaction);
        }
    }
}
