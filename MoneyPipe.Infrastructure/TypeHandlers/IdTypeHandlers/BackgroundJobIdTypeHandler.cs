using MoneyPipe.Domain.BackgroundJobAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.TypeHandlers.IdTypeHandlers
{
    public sealed class BackgroundJobIdTypeHandler : EntityIdTypeHandler<BackgroundJobId, Guid>
    {
        protected override BackgroundJobId Create(Guid value) => BackgroundJobId.CreateUnique(value);
        protected override Guid GetValue(BackgroundJobId id) => id.Value;
    }
}