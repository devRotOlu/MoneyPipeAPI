using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Application.Services.KYCManagement.Commands.AddDocument
{
    public class AddDocumentCommandHandler(IHttpContextAccessor httpContextAccessor,
    IUserReadRepository userQuery,IUnitOfWork unitofWork) 
    : IRequestHandler<AddDocumentCommand, ErrorOr<Success>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUserReadRepository _userQuery = userQuery;
        private readonly IUnitOfWork _unitofWork = unitofWork;
        public async Task<ErrorOr<Success>> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {
            var _userId = _httpContextAccessor.HttpContext.User.FindFirstValue(
                ClaimTypes.NameIdentifier);
            var userIdResult = UserId.CreateUnique(Guid.Parse(_userId));
            var user = await _userQuery.GetUserProfileAsync(userIdResult.Value);

            var profile = user?.KYCProfile;

            if (profile is null) return Errors.KYCManagement.MissingProfile;

            user!.AddKYCProfileDocument(request.Category,request.Type,request.Value,
            request.Issuer);
            await _unitofWork.Users.AddKycDocumentAsync(user);
            await _unitofWork.Commit();

            return Result.Success;
        }
    }
}