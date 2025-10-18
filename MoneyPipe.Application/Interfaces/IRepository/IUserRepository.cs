using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Interfaces.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
