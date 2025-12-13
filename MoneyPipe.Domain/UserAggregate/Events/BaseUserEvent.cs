using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Events
{
    public abstract class  BaseUserEvent(UserId userId,string email,string firstName,string? pageURL)
    :DomainEvent
    {
        public UserId UserId { get; } = userId;
        public string Email { get; } = email;
        public string FirstName {get;} = firstName;
        public string? PageURL {get;} = pageURL;
    }
}