using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.BackgroundJobAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Reads
{
    public class BackgroundJobReadRepository(IDbConnection dbConnection):IBackgroundJobReadRepository
    {
         private readonly IDbConnection _dbConnection = dbConnection;
         private readonly string _tableName = "BackgroundJobs";

        public async Task<IEnumerable<BackgroundJob>> GetUnCompletedBackgroundJobsAsync(string type)
        {
            var sql = $@"
                SELECT * 
                FROM {_tableName} 
                WHERE type = @Type AND isCompleted = @IsCompleted";

            return await _dbConnection.QueryAsync<BackgroundJob>(sql,
                new { Type = type, IsCompleted = false });
        }
    }
}