using Dapper;
using MoneyPipe.Application.Interfaces.IRepository;
using System.Data;

namespace MoneyPipe.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection _dbConnection;
        protected readonly IDbTransaction _transaction;
        protected readonly string _tableName;

        public GenericRepository(IDbConnection dbConnection, IDbTransaction transaction, string tableName)
        {
            _dbConnection = dbConnection;
            _transaction = transaction;
            _tableName = tableName;
        }
        public async Task<int> AddAsync(T entity)
        {
            var props = typeof(T).GetProperties().Where(p => p.Name != "Id");
            var columns = string.Join(", ", props.Select(p => p.Name.ToLowerInvariant()));
            var parameters = string.Join(", ", props.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters})";
            return await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_tableName}";
            return await _dbConnection.QueryAsync<T>(sql);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var props = typeof(T).GetProperties().Where(p => p.Name != "Id");
            var setClause = string.Join(", ", props.Select(p => $"{p.Name.ToLowerInvariant()} = @{p.Name}"));

            var sql = $"UPDATE {_tableName} SET {setClause} WHERE id = @Id";
            return await _dbConnection.ExecuteAsync(sql, entity);
        }
    }
}
