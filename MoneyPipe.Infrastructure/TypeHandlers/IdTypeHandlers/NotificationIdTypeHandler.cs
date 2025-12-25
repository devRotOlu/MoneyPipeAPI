using MoneyPipe.Domain.NotificationAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.TypeHandlers.IdTypeHandlers
{
    public sealed class NotificationIdTypeHandler : EntityIdTypeHandler<NotificationId, Guid>
    {
        protected override NotificationId Create(Guid value) => NotificationId.CreateUnique(value);
        protected override Guid GetValue(NotificationId id) => id.Value;
    }
}