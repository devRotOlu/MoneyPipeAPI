using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Domain.EmailJobAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Writes
{
    public class EmailJobWriteRepository(IDbConnection dbConnection,IDbTransaction transaction)
    :IEmailJobWriteRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly IDbTransaction _transaction = transaction;
        private readonly string _emailJobTable = "EmailJobs";

        public async Task CreateEmailJobAsync(EmailJob emailJob)
        {
            var sql= @$"
                INSERT INTO {_emailJobTable} (id, email, subject, message,
                status, attempts, createdat, updatedat,htmlcontent)
                VALUES (@Id, @Email, @Subject,@Message,@Status, 
                @Attempts,@CreatedAt,@UpdatedAt,@HtmlContent);";
            await _dbConnection.ExecuteAsync(sql, emailJob, _transaction);
        }
    }
}