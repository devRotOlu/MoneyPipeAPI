using MediatR;
using MoneyPipe.Domain.UserAggregate.Events;

namespace MoneyPipe.Application.Services.Authentication.Notifications
{
    public class UserRegisteredNotification(UserRegisteredEvent domainEvent):INotification
    {
        public UserRegisteredEvent DomainEvent {get;} = domainEvent;
    }
}