using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.UserAggregate.ValueObjects
{
    public abstract class BaseToken : ValueObject
    {
        protected BaseToken(string token, DateTime expiresAt, DateTime createdAt,UserId userId)
        {
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = createdAt;
            UserId = userId;
        }

        protected BaseToken()
        {
            
        }

        public UserId UserId { get; private set; }
        public string Token { get; private set; } 
        public DateTime ExpiresAt { get; private set; } 

        public DateTime CreatedAt { get; } 

        protected sealed override IEnumerable<object> GetEqualityComponents()
        {
            yield return Token;
            yield return ExpiresAt;
            yield return CreatedAt;
        }
    }
}