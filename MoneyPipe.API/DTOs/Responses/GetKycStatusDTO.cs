using MoneyPipe.Domain.UserAggregate.Enums;

namespace MoneyPipe.API.DTOs.Responses
{
    public record GetKycStatusDTO(KYCStatus ProfileStatus,DateTime? VerifiedAt);
}