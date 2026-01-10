using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers.IdTypeHandlers
{
    public sealed class KYCDocumentIdTypeHandler : BaseTypeHandler<KYCDocumentId, Guid>

    {
        protected override KYCDocumentId Create(Guid value) => KYCDocumentId.CreateUnique(value).Value;
        protected override Guid GetValue(KYCDocumentId id) => id.Value;
    }
}