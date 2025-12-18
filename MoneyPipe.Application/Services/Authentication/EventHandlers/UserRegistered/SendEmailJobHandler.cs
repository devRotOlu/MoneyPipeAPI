using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Common.Events;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Domain.EmailJobAggregate;
using MoneyPipe.Domain.UserAggregate.Events;

namespace MoneyPipe.Application.Services.Authentication.EventHandlers.UserRegistered
{
    public class SendEmailJobHandler(IEmailTemplateService emailTemplateService,
     IServiceProvider serviceProvider) : INotificationHandler<DomainEventNotification<UserRegisteredEvent>>
    {
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task Handle(DomainEventNotification<UserRegisteredEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            var userId = domainEvent.UserId.Value.ToString();
            var emailOption = _emailTemplateService.BuildEmailConfirmationEmail(userId,domainEvent.FirstName,
            domainEvent.EmailConfirmationToken,domainEvent.Email,domainEvent.PageURL);

            var result = EmailJob.Create(emailOption.ToEmail,emailOption.Message,emailOption.Subject);
            if (result.IsError) Console.WriteLine("");
            var emailJob = result.Value;
            emailJob.AddHTMLContent(emailOption.HtmlContent!);

            using var scope = _serviceProvider.CreateScope();
            var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await _unitOfWork.EmailJobs.CreateEmailJobAsync(emailJob);
            await _unitOfWork.Commit();
        }
    }
}