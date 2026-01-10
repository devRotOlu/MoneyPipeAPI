using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
    public sealed class KYCProfileIdTypeHandler : BaseTypeHandler<KYCProfileId, Guid>
    {
        protected override KYCProfileId Create(Guid value) => KYCProfileId.CreateUnique(value).Value;
        protected override Guid GetValue(KYCProfileId id) => id.Value;
    }
}