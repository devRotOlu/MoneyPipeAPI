using Dapper;
using Dapper.Contrib.Extensions;
using MoneyPipe.Application.Interfaces.IRepository;
using System.Data;
using System.Data.Common;

namespace MoneyPipe.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection _dbConnection;
        protected readonly IDbTransaction _transaction;
        protected  readonly string _tableName;


        public GenericRepository(IDbConnection dbConnection, IDbTransaction transaction,string tableName)
        {
            _dbConnection = dbConnection;
            _transaction = transaction;
            _tableName = tableName;
        }
        public async Task<int> AddAsync(T entity)
        {
            //return await _dbConnection.InsertAsync(entity, _transaction);

            var props = typeof(T).GetProperties().Where(p => p.Name != "Id");
            var columns = string.Join(", ", props.Select(p => p.Name.ToLowerInvariant()));
            var parameters = string.Join(", ", props.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters})";
            return await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            //var entity = await _dbConnection.GetAsync<T>(id);
            //if (entity == null) return false;
            //return await _dbConnection.DeleteAsync(entity, _transaction);
            var sql = $"DELETE FROM {_tableName} WHERE id = @Id";
            return await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            //return await _dbConnection.GetAllAsync<T>();
            var sql = $"SELECT * FROM {_tableName}";
            return await _dbConnection.QueryAsync<T>(sql);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            //return await _dbConnection.GetAsync<T>(id);
            var sql = $"SELECT * FROM {_tableName} WHERE id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<int> UpdateAsync(T entity)
        {
            //return await _dbConnection.UpdateAsync(entity, _transaction);
            var props = typeof(T).GetProperties().Where(p => p.Name != "Id");
            var setClause = string.Join(", ", props.Select(p => $"{p.Name.ToLowerInvariant()} = @{p.Name}"));

            var sql = $"UPDATE {_tableName} SET {setClause} WHERE id = @Id";
            return await _dbConnection.ExecuteAsync(sql, entity);
        }
    }
}
