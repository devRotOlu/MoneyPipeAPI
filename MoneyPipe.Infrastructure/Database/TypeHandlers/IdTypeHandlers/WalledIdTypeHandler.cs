using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
    public sealed class WalletIdTypeHandler : BaseTypeHandler<WalletId, Guid>
    {
        protected override WalletId Create(Guid value) => WalletId.CreateUnique(value).Value;
        protected override Guid GetValue(WalletId id) => id.Value;
    }
}