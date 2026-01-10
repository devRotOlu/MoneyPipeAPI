using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.NotificationAggregate;
using MoneyPipe.Domain.UserAggregate;
using MoneyPipe.Domain.UserAggregate.Entities;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Reads
{
    public class UserReadRepository(IDbConnection dbConnection): IUserReadRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly string _userTable = "Users";
        private readonly string _refreshTokenTable = "RefreshTokens";
        private readonly string _resetTokenTable = "PasswordResetTokens";
        private readonly string _notificationTable = "Notifications";
        private readonly string _kycProfileTable = "KycProfiles";
        private readonly string _kycDocumentTable = "KycDocuments";

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(UserId userId)
        {

            var sql = @$"
                SELECT id, title, message, metadatajson, type, 
                isread, readat ,createdat,
                FROM {_notificationTable}
                WHERE userid = @UserId AND isread = false
                ORDER BY created_at DESC;
            ";

            return await _dbConnection.QueryAsync<Notification>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsByUserIdAsync(Guid userId)
        {
            var sql = @$"
                SELECT id, title, message, metadatajson, type, 
                isread, createdat, readat
                FROM {_notificationTable}
                WHERE userid = @UserId
                ORDER BY createdat DESC;
            ";
            return await _dbConnection.QueryAsync<Notification>(sql, new { UserId = userId });
        }

        public async Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token,UserId userId)
        {
            var sql = @$"
            SELECT token, expiresat, isused, createdat
            FROM {_resetTokenTable}
            WHERE token = @Token AND userid = @UserId";

            return await _dbConnection.QueryFirstOrDefaultAsync<PasswordResetToken>(sql, 
            new { Token = token,UserId = userId });
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        {
            var sql = @$"
            SELECT token, expiresat, revokedat, createdat, userid
            FROM {_refreshTokenTable}
            WHERE token = @Token";

            return await _dbConnection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql = @$"SELECT id, email, passwordHash, firstName, lastName,
                      emailConfirmed, defaultCurrency, emailConfirmed, emailConfirmationToken,
                      emailConfirmationExpiry
                      FROM {_userTable} WHERE email = @Email";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User?> GetUserByIdAsync(UserId id)
        {
            var sql = @$"SELECT id, email, passwordHash, firstName, lastName,
                      emailConfirmed, defaultCurrency, emailConfirmed, emailConfirmationToken,
                      emailConfirmationExpiry
                      FROM {_userTable} WHERE id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetUserProfileAsync(UserId userId)
        {
            var sql = @$"SELECT id from {_userTable} WHERE Id = @UserId;
            SELECT status, verifiedat FROM {_kycProfileTable} WHERE
            userid = @UserId;
            SELECT * FROM {_kycDocumentTable} WHERE userid = @UserId
            ";
            using var multi = await _dbConnection.QueryMultipleAsync(sql,new {UserId = userId});

            var user = await multi.ReadFirstOrDefaultAsync<User>();

            if (user is null) return null;

            var profile = await multi.ReadFirstOrDefaultAsync<KYCProfile>();

            if (profile is null) return user;

            user.AddKYCProfile(profile);

            var documents = (await multi.ReadAsync<KYCDocument>()).ToList();

            user.KYCProfile.AddKYCDocuments(documents);

            return user;
        }
    }
}