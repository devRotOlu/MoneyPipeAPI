using MoneyPipe.Domain.BackgroundJobAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
    public sealed class BackgroundJobIdTypeHandler : BaseTypeHandler<BackgroundJobId, Guid>
    {
        protected override BackgroundJobId Create(Guid value) => BackgroundJobId.CreateUnique(value);
        protected override Guid GetValue(BackgroundJobId id) => id.Value;
    }
}