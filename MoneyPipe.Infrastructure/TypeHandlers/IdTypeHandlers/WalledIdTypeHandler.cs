using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.TypeHandlers.IdTypeHandlers
{
    public sealed class WalletIdTypeHandler : EntityIdTypeHandler<WalletId, Guid>
    {
        protected override WalletId Create(Guid value) => WalletId.CreateUnique(value).Value;
        protected override Guid GetValue(WalletId id) => id.Value;
    }
}