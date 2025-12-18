using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.UserAggregate;
using MoneyPipe.Domain.UserAggregate.Models;


namespace MoneyPipe.Application.Services.Authentication.Commands.Register
{
    class RegisterCommandHandler:IRequestHandler<RegisterCommand,ErrorOr<Success>>
    {
        public RegisterCommandHandler(IUnitOfWork unitOfWork,IMapper mapper,
        IUserReadRepository userQuery,ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userQuery = userQuery;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _userQuery;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public async Task<ErrorOr<Success>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userQuery.GetUserByEmailAsync(request.Email);
            if (existingUser is not null) return Errors.User.DuplicateEmail;

            var userData = _mapper.Map<UserRegisterData>(request);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            userData.PasswordHash = hashedPassword;
            userData.EmailConfirmationToken = _tokenService.GenerateEmailConfirmationToken();

            var result = User.Create(userData);
            if (result.IsError) return result.Errors;

            var user = result.Value;
            var clientURL = _httpContextAccessor.HttpContext.Request.GetClientURL(_configuration["EmailConfirmation"]!);
            user.AddUserRegisteredDomainEvent(clientURL);
            
            await _unitOfWork.Users.CreateUserAsync(user);
            await _unitOfWork.RegisterAggregateAsync(user);
            await _unitOfWork.Commit();

            return Result.Success;
        }
    }
}