using MoneyPipe.Domain.UserAggregate.Enums;

namespace MoneyPipe.Application.Services.KYCManagement.Queries.GetKycStatus
{
    public record GetKycSatusResult(KYCStatus ProfileStatus,DateTime? VerifiedAt);
}