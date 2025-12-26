using MoneyPipe.Domain.UserAggregate.Entities.Common;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Entities
{
    public class RefreshToken : BaseToken<RefreshTokenId>
    {
        private RefreshToken(RefreshTokenId id)
        :base(id)
        {
            
        }
        
        private RefreshToken(){}

        public DateTime? RevokedAt { get; private set; } 
        internal static RefreshToken Create(string token, DateTime expiresAt,UserId userId,RefreshTokenId id)
        {
            return new(id)
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserId = userId
            };
        }

        internal void MarkAsRevoked()=> RevokedAt = DateTime.UtcNow;
    }
}
