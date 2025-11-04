using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.IRepository;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Infrastructure.Repository
{
        public class PasswordResetRepository : GenericRepository<PasswordResetToken>, IPasswordResetRepository
    {

        public PasswordResetRepository(IDbConnection dbConnection, IDbTransaction transaction) : base(dbConnection, transaction, "password_reset_tokens")
            { }

        public async Task<PasswordResetToken?> GetByTokenAndUserIdAsync(string token,Guid userId)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<PasswordResetToken>($"SELECT * FROM {_tableName} WHERE Token = @Token AND UserId = @UserId", new { Token = token,UserId = userId});
        }

        public async Task MarkAsUsedAsync(Guid id)
        {
            await _dbConnection.ExecuteAsync( $"UPDATE {_tableName} SET IsUsed = TRUE WHERE Id = @Id", new { Id = id });
        }

    }
}