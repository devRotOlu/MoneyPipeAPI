using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
     public sealed class AccountIdTypeHandler : BaseTypeHandler<VirtualAccountId, Guid>
    {
        protected override VirtualAccountId Create(Guid value) => VirtualAccountId.CreateUnique(value).Value;
        protected override Guid GetValue(VirtualAccountId id) => id.Value;
    }
}