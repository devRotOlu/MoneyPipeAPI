using MoneyPipe.Domain.BackgroundJobAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Persistence.Configurations.IdTypeHandlers
{
    public sealed class BackgroundJobIdTypeHandler : EntityIdTypeHandler<BackgroundJobId, Guid>
    {
        protected override BackgroundJobId Create(Guid value) => BackgroundJobId.CreateUnique(value);
        protected override Guid GetValue(BackgroundJobId id) => id.Value;
    }
}