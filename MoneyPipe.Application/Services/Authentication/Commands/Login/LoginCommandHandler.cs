using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services.Authentication.Common;
using MoneyPipe.Domain.Common.Errors;

namespace MoneyPipe.Application.Services.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<AuthenticationResult>>
    {
        public LoginCommandHandler(IMapper mapper,IUserReadRepository userQuery,
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
        
        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userQuery.GetUserByEmailAsync(request.Email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) 
                return Errors.Authentication.InvalidCredentials;

            if (!user.EmailConfirmed) return Errors.EmailConfirmation.UnConfirmed;

            var refreshToken = _tokenService.GetRefreshToken();

            var result = user.AddRefreshToken(refreshToken);

            if (result.IsError) Console.WriteLine("") ;

            var refreshTokenExpirationTime = result.Value;

            _tokenService.SetTokenInCookies(user,refreshToken,refreshTokenExpirationTime,_httpContextAccessor.HttpContext);

            await _unitOfWork.Users.AddRefreshTokenAsync(user);
            _unitOfWork.Commit();

            var loginResult = _mapper.Map<AuthenticationResult>(user);

            return loginResult;
        }
    }
}