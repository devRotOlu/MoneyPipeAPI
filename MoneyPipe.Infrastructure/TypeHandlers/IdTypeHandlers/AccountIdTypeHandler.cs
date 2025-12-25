using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.TypeHandlers.IdTypeHandlers
{
     public sealed class AccountIdTypeHandler : EntityIdTypeHandler<VirtualAccountId, Guid>
    {
        protected override VirtualAccountId Create(Guid value) => VirtualAccountId.CreateUnique(value).Value;
        protected override Guid GetValue(VirtualAccountId id) => id.Value;
    }
}