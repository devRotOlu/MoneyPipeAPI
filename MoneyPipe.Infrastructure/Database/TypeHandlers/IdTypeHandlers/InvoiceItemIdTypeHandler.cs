using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
    public sealed class InvoiceItemIdTypeHandler : BaseTypeHandler<InvoiceItemId, Guid>
    {
        protected override InvoiceItemId Create(Guid value) => InvoiceItemId.CreateUnique(value);
        protected override Guid GetValue(InvoiceItemId id) => id.Value;
    }
}