using MoneyPipe.Domain.EmailJobAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.TypeHandlers.IdTypeHandlers
{
    public sealed class EmailJobIdTypeHandler : EntityIdTypeHandler<EmailJobId, Guid>
    {
        protected override EmailJobId Create(Guid value) => EmailJobId.CreateUnique(value);
        protected override Guid GetValue(EmailJobId id) => id.Value;
    }
}