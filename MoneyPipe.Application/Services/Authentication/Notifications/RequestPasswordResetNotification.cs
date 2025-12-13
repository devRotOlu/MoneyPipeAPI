using MediatR;
using MoneyPipe.Domain.UserAggregate.Events;

namespace MoneyPipe.Application.Services.Authentication.Notifications
{
    public class RequestPasswordResetNotification(PasswordResetRequestedEvent domainEvent)
    :INotification
    {
        public PasswordResetRequestedEvent DomainEvent {get;} = domainEvent;
    }
}