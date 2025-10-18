using MoneyPipe.Application.Interfaces.IRepository;

namespace MoneyPipe.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; set; }
        IRefreshTokenRepository RefreshTokens { get; set; }
        void CommitAsync();
        void RollbackAsync();
    }
}
