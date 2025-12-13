using MoneyPipe.Domain.EmailJobAggregate;

namespace MoneyPipe.Application.Interfaces.Persistence.Writes
{
    public interface IEmailJobWriteRepository
    {
        Task CreateEmailJobAsync(EmailJob emailJob);

    }
}