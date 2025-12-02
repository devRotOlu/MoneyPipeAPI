

using MoneyPipe.Domain.UserAggregate;

namespace MoneyPipe.Application.Interfaces.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
