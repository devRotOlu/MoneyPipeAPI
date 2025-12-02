using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Domain.UserAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Writes
{
    public class UserWriteRepository(IDbConnection dbConnection,IDbTransaction transaction) : IUserWriteRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly IDbTransaction _transaction = transaction;
        private readonly string _userTable = "Users";
        private readonly string _refreshTokenTable = "RefreshTokens";
        private readonly string _resetTokenTable = "PasswordResetTokens";
        private readonly string _notificationTable = "Notifications";
        private readonly string _emailJobTable = "EmailJobs";

        public async Task CreateUserAsync(User user)
        {
            var sql= @$"
            INSERT INTO {_userTable} (id, email, firstname,lastname, passwordhash, 
            createdat, updatedat, emailconfirmationtoken, defaultcurrency, emailconfirmationexpiry)
            VALUES (@Id, @Email, @Firstname,@Lastname,@PasswordHash, 
            @CreatedAt,@UpdatedAt,@EmailConfirmationToken, @DefaultCurrency,@EmailConfirmationExpiry);";
            await _dbConnection.ExecuteAsync(sql, user, _transaction);
        }

        public async Task UpdateUserPassword(User user)
        {
            var sql = @$"UPDATE {_userTable} SET
                    passwordhash = @PasswordHash
                    WHERE id = @Id";
            await _dbConnection.ExecuteAsync(sql, user, _transaction);
        }

        public async Task MarkConfirmedEmail(User user)
        {
            var sql = @$"UPDATE {_userTable} SET 
                    emailconfirmed = @EmailConfirmed,
                    emailconfirmationtoken = @EmailConfirmationToken,
                    emailconfirmationexpiry = @EmailConfirmationExpiry,
                    updatedat = @UpdatedAt
                    WHERE id = @Id";
            await _dbConnection.ExecuteAsync(sql,user,_transaction);
        }

        public async Task AddPasswordResetTokenAsync(User user)
        {
            var sql = @$"
                INSERT INTO {_resetTokenTable} (userid,token,expiresat,isused,createdat)
                VALUES (@UserId,@Token,@ExpiresAt,@IsUsed,@CreatedAt)";
            await _dbConnection.ExecuteAsync(sql,user.PasswordResetToken,_transaction);
        }

        public async Task MarkPasswordResetTokenAsUsedAsync(User user)
        {
            var sql = @$"UPDATE {_resetTokenTable} SET
                    isused = @IsUsed
                    WHERE token = @Token and userid = @UserId";
            await _dbConnection.ExecuteAsync(sql,user.PasswordResetToken,_transaction);
        }

        public async Task AddRefreshTokenAsync(User user)
        {
            var sql = @$"
                INSERT INTO {_refreshTokenTable} (userid, token, createdat, expiresat, revokedat)
                VALUES (@UserId, @Token, @CreatedAt, @ExpiresAt, @RevokedAt);
            ";

            await _dbConnection.ExecuteAsync(sql, user.RefreshToken, _transaction);
        }

        public async Task RevokeRefreshTokenAsync(User user)
        {
            var sql = @$"
                UPDATE {_refreshTokenTable}
                SET revokedat = @RevokedAt
                WHERE token = @Token AND userid = @UserId;
            ";
            await _dbConnection.ExecuteAsync(sql,user.RefreshToken,_transaction);
        }

        public async Task CreateEmailJobAsync(User user)
        {
            var sql= @$"
            INSERT INTO {_emailJobTable} (id, email, subject, message,
            status, attempts, createdat, updatedat,htmlcontent)
            VALUES (@Id, @Email, @Subject,@Message,@Status, 
            @Attempts,@CreatedAt,@UpdatedAt,@HtmlContent);";
            await _dbConnection.ExecuteAsync(sql, user.EmailJob, _transaction);
        }
    }
}