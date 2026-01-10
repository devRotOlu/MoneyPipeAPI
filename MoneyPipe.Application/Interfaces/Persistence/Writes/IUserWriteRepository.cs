using MoneyPipe.Domain.UserAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Writes
{
    public interface IUserWriteRepository
    {
        Task CreateUserAsync(User user);
        Task AddRefreshTokenAsync(User user);
        Task RevokeRefreshTokenAsync(User user);
        Task MarkConfirmedEmail(User user);
        Task AddPasswordResetTokenAsync(User user);
        Task UpdateUserPassword(User user);
        Task MarkPasswordResetTokenAsUsedAsync(User user);
        Task AddKycProfileAsync(User user);
        Task AddKycDocumentAsync(User user);
    }
}