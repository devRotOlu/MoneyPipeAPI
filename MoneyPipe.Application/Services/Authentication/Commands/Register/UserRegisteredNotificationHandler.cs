using MediatR;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Services.Authentication.Notifications;
using MoneyPipe.Domain.EmailJobAggregate;

namespace MoneyPipe.Application.Services.Authentication.Commands.Register
{
    public class UserRegisteredNotificationHandler(IEmailTemplateService emailTemplateService,
     IUnitOfWork unitOfWork) : INotificationHandler<UserRegisteredNotification>
    {
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            var userId = domainEvent.UserId.Value.ToString();
            var emailOption = _emailTemplateService.BuildEmailConfirmationEmail(userId,domainEvent.FirstName,
            domainEvent.EmailConfirmationToken,domainEvent.Email,domainEvent.PageURL);

            var result = EmailJob.Create(emailOption.Subject,emailOption.Message,emailOption.ToEmail);
            if (result.IsError) Console.WriteLine("");
            var emailJob = result.Value;
            emailJob.AddHTMLContent(emailOption.HtmlContent!);

            await _unitOfWork.EmailJobs.CreateEmailJobAsync(emailJob);   
        }
    }
}