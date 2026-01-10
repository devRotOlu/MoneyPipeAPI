using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
    public sealed class InvoiceIdTypeHandler : BaseTypeHandler<InvoiceId, Guid>
    {
        protected override InvoiceId Create(Guid value) => InvoiceId.CreateUnique(value).Value;
        protected override Guid GetValue(InvoiceId id) => id.Value;
    }
}