using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Entities.Common
{
    public abstract class BaseToken<TId> : Entity<TId> where TId: notnull
    {
        protected BaseToken(TId id):base(id)
        {
            CreatedAt = DateTime.Now;
        }

        protected BaseToken()
        {
            
        }

        public UserId UserId { get; protected set; }
        public string Token { get; protected set; } 
        public DateTime ExpiresAt { get; protected set; } 

        public DateTime CreatedAt { get; } 
    }
}