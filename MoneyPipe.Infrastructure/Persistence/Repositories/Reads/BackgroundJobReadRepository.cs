using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.BackgroundJobAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Reads
{
    public class BackgroundJobReadRepository(IDbConnection dbConnection):IBackgroundJobReadRepository
    {
         private readonly IDbConnection _dbConnection = dbConnection;
         private readonly string _tableName = "BackgroundJob";

        public async Task<IEnumerable<BackgroundJob>> GetUnCompletedBackgroundJobsAsync(string type)
        {
            var sql = @$" * FROM {_tableName} 
            WHERE type = @Type AND status != @Status";
            return await _dbConnection.QueryAsync<BackgroundJob>(sql,
             new { Type = type,Status = "Completed"});
        }
    }
}