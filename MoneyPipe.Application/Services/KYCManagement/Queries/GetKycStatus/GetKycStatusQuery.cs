using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.KYCManagement.Queries.GetKycStatus
{
    public class GetKycStatusQuery():IRequest<ErrorOr<GetKycSatusResult>>;
}