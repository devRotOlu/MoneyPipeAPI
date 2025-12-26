using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services.Authentication.Common;
using MoneyPipe.Domain.Common.Errors;

namespace MoneyPipe.Application.Services.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthenticationResult>>
    {
        public RefreshTokenCommandHandler(IMapper mapper,IUserReadRepository userQuery,
        IUnitOfWork unitOfWork,ITokenService tokenService,IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;   
            _userQuery = userQuery;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IMapper _mapper;
        private readonly IUserReadRepository _userQuery;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public async Task<ErrorOr<AuthenticationResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var oldRefreshToken = _tokenService.RetrieveOldRefreshToken(_httpContextAccessor.HttpContext);
            var stored = await _userQuery.GetRefreshTokenByTokenAsync(oldRefreshToken);
            if (stored is null || stored.RevokedAt != null || stored.ExpiresAt.CompareTo(DateTime.UtcNow) <= 0)
                return Errors.RefreshToken.InvalidToken;

            var user = await _userQuery.GetUserByIdAsync(stored.UserId);
            user!.RevokeRefreshToken(stored);

            // rotate refresh token: revoke old, create new
            await _unitOfWork.Users.RevokeRefreshTokenAsync(user);
            await _unitOfWork.Commit();

            var authResult = _mapper.Map<AuthenticationResult>(user);

            return authResult;
        }
    }
}