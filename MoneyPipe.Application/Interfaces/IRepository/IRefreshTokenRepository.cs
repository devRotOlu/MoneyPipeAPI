using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Interfaces.IRepository
{
    public interface IRefreshTokenRepository:IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task RevokeAsync(Guid id);
        Task RevokeAllForUserAsync(Guid userId);
    }
}
