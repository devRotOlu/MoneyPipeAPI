using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Interfaces.IRepository
{
    public interface IPasswordResetRepository:IGenericRepository<PasswordResetToken>
    {
        Task<PasswordResetToken?> GetByTokenAndUserIdAsync(string token,Guid userId);
        Task MarkAsUsedAsync(Guid id);
    }
}

