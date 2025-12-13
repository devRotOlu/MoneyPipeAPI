using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Events
{
    public sealed class UserRegisteredEvent(UserId userId,string email,
    string emailConfirmationToken,string firstName,string? clientURL) 
    : BaseUserEvent(userId,email,firstName,clientURL)
    {
        public string EmailConfirmationToken {get;} = emailConfirmationToken;
    }
}