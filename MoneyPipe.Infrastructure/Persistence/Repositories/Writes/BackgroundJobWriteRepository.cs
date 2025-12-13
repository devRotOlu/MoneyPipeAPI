using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Domain.BackgroundJobAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Writes
{
    class BackgroundJobWriteRepository(IDbConnection dbConnection,IDbTransaction transaction)
    :IBackgroundJobWriteRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly IDbTransaction _transaction = transaction;
        private readonly string _tableName = "BackgroundJobs";

        public async Task CreateBackgroundJobAsync(BackgroundJob job)
        {
            var sql = @$"
            INSERT INTO {_tableName} (id, type, invoiceid, status, createdat, updatedat)
            VALUES (@Id, @Type, @InvoiceId, @Status,@CreatedAt, @UpdatedAt);
            ";
            await _dbConnection.ExecuteAsync(sql,job,_transaction);
        }

        public async Task UpdateBackgroundJobAsync(BackgroundJob job)
        {
            var sql = @$"UPDATE {_tableName} 
            SET status = @Status,
            attempts = @Attempts,
            updatedat = @UpdatedAt
            WHERE id = @Id";
            await _dbConnection.ExecuteAsync(sql,job,_transaction);
        }
    }
}