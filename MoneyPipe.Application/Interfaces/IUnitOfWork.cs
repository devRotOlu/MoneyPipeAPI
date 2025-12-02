using MoneyPipe.Application.Interfaces.Persistence.Writes;

namespace MoneyPipe.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // IUserRepository Users { get; set; }
        // IRefreshTokenRepository RefreshTokens { get; set; }
        // IPasswordResetRepository PasswordRestTokens { get; set; }
        // IInvoiceRepository Invoices { get; set; }
        public IUserWriteRepository Users { get; set; }
        void Commit();
        void Rollback();
    }
}
