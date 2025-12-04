using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Persistence.Configurations.IdTypeHandlers
{
    public sealed class InvoiceIdTypeHandler : EntityIdTypeHandler<InvoiceId, Guid>
    {
        protected override InvoiceId Create(Guid value) => InvoiceId.CreateUnique(value);
        protected override Guid GetValue(InvoiceId id) => id.Value;
    }
}