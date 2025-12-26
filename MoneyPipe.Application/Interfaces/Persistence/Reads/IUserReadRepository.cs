using MoneyPipe.Domain.NotificationAggregate;
using MoneyPipe.Domain.UserAggregate;
using MoneyPipe.Domain.UserAggregate.Entities;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Application.Interfaces.Persistence.Reads
{
    public interface IUserReadRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
        Task<User?> GetUserByIdAsync(UserId id);
        Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token,UserId userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(UserId userId);
    }
}