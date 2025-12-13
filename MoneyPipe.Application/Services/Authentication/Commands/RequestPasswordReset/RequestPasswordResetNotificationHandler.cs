using MediatR;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Services.Authentication.Notifications;
using MoneyPipe.Domain.EmailJobAggregate;

namespace MoneyPipe.Application.Services.Authentication.Commands.RequestPasswordReset
{
    class RequestPasswordResetNotificationHandler(IEmailTemplateService emailTemplateService,
     IUnitOfWork unitOfWork) : INotificationHandler<RequestPasswordResetNotification>
    {
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Handle(RequestPasswordResetNotification notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            var userId = domainEvent.UserId.Value.ToString();

            var option = _emailTemplateService.BuildForgotPasswordEmail(userId,domainEvent.FirstName,
            domainEvent.PasswordResetToken,domainEvent.Email,domainEvent.PageURL);
            var result = EmailJob.Create(domainEvent.Email,option.Message,option.Subject);
            
            if(result.IsError) Console.WriteLine("");

            var emailJob = result.Value;
            emailJob.AddHTMLContent(option.HtmlContent!);
            await _unitOfWork.EmailJobs.CreateEmailJobAsync(emailJob);    
        }
    }
}