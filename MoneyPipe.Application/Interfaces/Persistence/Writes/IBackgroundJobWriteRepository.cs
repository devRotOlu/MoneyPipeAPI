using MoneyPipe.Domain.BackgroundJobAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Writes
{
    public interface IBackgroundJobWriteRepository
    {
        Task CreateBackgroundJobAsync(BackgroundJob job);
        Task UpdateBackgroundJobAsync(BackgroundJob job);
    }
}