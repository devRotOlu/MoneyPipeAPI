namespace MoneyPipe.Infrastructure.Repository
{
    using Dapper;
    using MoneyPipe.Application.Interfaces.IRepository;
    using MoneyPipe.Domain.Entities;
    using System.Data;

    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IDbConnection dbConnection, IDbTransaction transaction) : base(dbConnection, transaction, "RefreshTokens")
        { }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<RefreshToken>($"SELECT * FROM {_tableName} WHERE Token = @Token", new { Token = token });
        }

        public async Task RevokeAsync(Guid id)
        {
            await _dbConnection.ExecuteAsync($"UPDATE {_tableName} SET RevokedAt = NOW() WHERE Id = @Id", new { Id = id }, _transaction);
        }

        public async Task RevokeAllForUserAsync(Guid userId)
        {
            await _dbConnection.ExecuteAsync($"UPDATE {_tableName} SET RevokedAt = NOW() WHERE UserId = @UserId AND RevokedAt IS NULL", new { UserId = userId }, _transaction);
        }
    }

}
