using MoneyPipe.Domain.NotificationAggregate;
using MoneyPipe.Domain.UserAggregate;
using MoneyPipe.Domain.UserAggregate.Entities;

namespace MoneyPipe.Application.Interfaces.Persistence.Reads
{
    public interface IUserReadRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token,Guid userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(Guid userId);
    }
}