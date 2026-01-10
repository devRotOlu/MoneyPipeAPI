using System.Security.Claims;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.UserAggregate.Models;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Application.Services.KYCManagement.Commands.CompleteProfile
{
    public class CompleteProfileCommandHandler(IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,IUserReadRepository userQuery,
    IMapper mapper) :
     IRequestHandler<CompleteProfileCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitofWork = unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUserReadRepository _userQuery = userQuery;
        private readonly IMapper _mapper = mapper;
        public async Task<ErrorOr<Success>> Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
        {
            var _userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userIdResult = UserId.CreateUnique(Guid.Parse(_userId));
            var user = await _userQuery.GetUserByIdAsync(userIdResult.Value);
            
            var data = _mapper.Map<KYCData>(request);
            user!.AddKYCProfile(data);
            user.AddKYCProfileDocument(request.NIN.Category,"NIN",request.NIN.Value,"National Identity Management Commission (NIMC)");

            await _unitofWork.Users.AddKycProfileAsync(user);
            await _unitofWork.Users.AddKycDocumentAsync(user);
            await _unitofWork.Commit();

            return Result.Success;
        }
    }
}