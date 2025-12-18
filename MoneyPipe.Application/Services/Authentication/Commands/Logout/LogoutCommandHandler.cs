using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;

namespace MoneyPipe.Application.Services.Authentication.Commands.Logout
{
    public class LogoutCommandHandler(IUnitOfWork unitOfWork, IUserReadRepository userQuery, 
    ITokenService tokenService,IHttpContextAccessor httpContextAccessor) 
    : IRequestHandler<LogoutCommand, Success>
    {
        private readonly IUnitOfWork _unitofWork = unitOfWork;
        private readonly IUserReadRepository _userQuery = userQuery;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<Success> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _tokenService.RetrieveOldRefreshToken(_httpContextAccessor.HttpContext);
            _tokenService.InvalidateTokenInCookies(_httpContextAccessor.HttpContext);
            var storedToken = await _userQuery.GetRefreshTokenByTokenAsync(refreshToken);

            var user = await _userQuery.GetUserByIdAsync(storedToken!.UserId.Value);
            user!.RevokeRefreshToken(storedToken);

            await _unitofWork.Users.RevokeRefreshTokenAsync(user);
            await _unitofWork.Commit();
            
            return Result.Success;
        }
    }
}