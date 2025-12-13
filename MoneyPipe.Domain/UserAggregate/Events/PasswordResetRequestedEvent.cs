using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Events
{
    public sealed class PasswordResetRequestedEvent(UserId userId,string email,
    string passwordResetToken,string firstName,string? resetLink)
    :BaseUserEvent(userId,email,firstName,resetLink)
    {
        public string PasswordResetToken {get;} = passwordResetToken;
    }   
}