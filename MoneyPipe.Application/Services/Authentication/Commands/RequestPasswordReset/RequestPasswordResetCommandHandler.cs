using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.Common.Errors;

namespace MoneyPipe.Application.Services.Authentication.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, ErrorOr<Success>>
    {
        public RequestPasswordResetCommandHandler(IUnitOfWork unitofWork,IHttpContextAccessor httpContextAccessor,
        IUserReadRepository userQuery,ITokenService tokenService,
        IEmailTemplateService emailTemplateService,IConfiguration configuration)
        {
            _unitofWork = unitofWork;
            _httpContextAccessor = httpContextAccessor;
            _userQuery = userQuery;
            _tokenService = tokenService;
            _emailTemplateService = emailTemplateService;
            _configuration = configuration;
        }

        private readonly IUnitOfWork _unitofWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserReadRepository _userQuery;
        private readonly ITokenService _tokenService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IConfiguration _configuration;
        public async Task<ErrorOr<Success>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _userQuery.GetUserByEmailAsync(request.Email);

            if (user is null) return Errors.User.NotFound;

            var token = _tokenService.GeneratePasswordResetToken();

            var result = user.SetPasswordResetToken(token);

            if (result.IsError) Console.WriteLine("");

            var resetLink = _httpContextAccessor.HttpContext.Request.GetClientURL(_configuration["PasswordResetRoute"]!);

            var option = _emailTemplateService.BuildForgotPasswordEmail(user.Id.Value.ToString(),user.FirstName,token,request.Email,resetLink!);

            user.AddEmailJob(option.Subject,option.Message,option.ToEmail);
            user.AddEmailJobHTMLContent(option.HtmlContent!);

            await _unitofWork.Users.AddPasswordResetTokenAsync(user);
            await _unitofWork.Users.CreateEmailJobAsync(user);
            _unitofWork.Commit();

            return Result.Success;
        }
    }
}