using MoneyPipe.Domain.NotificationAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
    public sealed class NotificationIdTypeHandler : BaseTypeHandler<NotificationId, Guid>
    {
        protected override NotificationId Create(Guid value) => NotificationId.CreateUnique(value);
        protected override Guid GetValue(NotificationId id) => id.Value;
    }
}