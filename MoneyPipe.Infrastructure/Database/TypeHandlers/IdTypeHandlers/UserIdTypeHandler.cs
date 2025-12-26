using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
    public sealed class UserIdTypeHandler : EntityIdTypeHandler<UserId, Guid>
    {
        protected override UserId Create(Guid value) => UserId.CreateUnique(value).Value;
        protected override Guid GetValue(UserId id) => id.Value;
    }
}