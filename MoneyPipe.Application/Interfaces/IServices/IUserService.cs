using ErrorOr;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<ErrorOr<User>> GetByIdAsync(Guid userId);
        Task<ErrorOr<User>> GetByEmailAsync(string email);
    }
}