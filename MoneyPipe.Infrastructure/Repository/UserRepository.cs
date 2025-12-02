namespace MoneyPipe.Infrastructure.Repository
{
    using Dapper;
    using MoneyPipe.Application.Interfaces.IRepository;
    using MoneyPipe.Domain.UserAggregate;
    using System.Data;

    public class UserRepository(IDbConnection dbConnection, IDbTransaction transaction) : GenericRepository<User>(dbConnection,transaction,"Users"), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<User>($"SELECT * FROM {_tableName} WHERE Email = @Email", new { Email = email });
        }

        public async Task SaveAsync(User user)
        {

            // Save main user
            await _dbConnection.ExecuteAsync(
                @"UPDATE users SET 
                    email = @Email,
                    first_name = @FirstName,
                    last_name = @LastName,
                    password_hash = @PasswordHash,
                    updated_at = NOW()
                WHERE id = @Id",
                new {
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.PasswordHash
                }, _transaction);

            // Save RefreshToken (if present)
            if (user.RefreshToken != null)
            {
                await _dbConnection.ExecuteAsync(
                    @"INSERT INTO refresh_tokens 
                    (id, user_id, token, expires_at, created_at, revoked_at)
                    VALUES (@Id, @UserId, @Token, @ExpiresAt, @CreatedAt, @RevokedAt)",
                    new {
                        Id = Guid.NewGuid(),
                        UserId = user.Id.Value,
                        user.RefreshToken.Token,
                        user.RefreshToken.ExpiresAt,
                        user.RefreshToken.CreatedAt,
                        user.RefreshToken.RevokedAt
                    }, _transaction);
            }

            // Save PasswordResetToken (if present)
            if (user.PasswordResetToken != null)
            {
                await _dbConnection.ExecuteAsync(
                    @"INSERT INTO password_reset_tokens
                    (id, user_id, token, expires_at, created_at, is_used)
                    VALUES (@Id, @UserId, @Token, @ExpiresAt, @CreatedAt, @IsUsed)",
                    new {
                        Id = Guid.NewGuid(),
                        user.Id.Value,
                        user.PasswordResetToken.Token,
                        user.PasswordResetToken.ExpiresAt,
                        user.PasswordResetToken.CreatedAt,
                        user.PasswordResetToken.IsUsed
                    }, _transaction);
            }

            // Save Notifications
            foreach (var n in user.Notifications)
            {
                await _dbConnection.ExecuteAsync(
                    @"INSERT INTO notifications 
                    (id, user_id, title, message, metadata_json, type, is_read, created_at)
                    VALUES (@Id, @UserId, @Title, @Message, @MetadataJson, @Type, @IsRead, @CreatedAt)",
                    new {
                        n.Id,
                        UserId = user.Id.Value,
                        n.Title,
                        n.Message,
                        n.MetadataJson,
                        n.Type,
                        n.IsRead,
                        n.CreatedAt
                    }, _transaction);
            }
        }
    }

}
