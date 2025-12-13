using MoneyPipe.Domain.BackgroundJobAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Reads
{
    public interface IBackgroundJobReadRepository
    {
        Task<IEnumerable<BackgroundJob>> GetUnCompletedBackgroundJobsAsync(string type);
    }
}