using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Application.Services.KYCManagement.Queries.GetKycStatus
{
    public class GetKycStatusQueryHandler(IUserReadRepository userQuery,
    IHttpContextAccessor httpContextAccessor) 
    : IRequestHandler<GetKycStatusQuery, ErrorOr<GetKycSatusResult>>
    {
        private readonly IUserReadRepository _userQuery = userQuery; 
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<ErrorOr<GetKycSatusResult>> Handle(GetKycStatusQuery request, CancellationToken cancellationToken)
        {
            var _userId = _httpContextAccessor.HttpContext.User.FindFirstValue(
                ClaimTypes.NameIdentifier);
            var userIdResult = UserId.CreateUnique(Guid.Parse(_userId));
            
            var user = await _userQuery.GetUserProfileAsync(userIdResult.Value);

            var profile = user?.KYCProfile;
            
            if (profile is null) return Errors.KYCManagement.MissingProfile;

            return new GetKycSatusResult(profile.Status,profile.VerifiedAt);
        }
    }
}