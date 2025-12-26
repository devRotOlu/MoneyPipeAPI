using MoneyPipe.Domain.UserAggregate.Entities.Common;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Entities
{
    public class PasswordResetToken : BaseToken<PasswordRefreshTokenId>
    {
        private PasswordResetToken(PasswordRefreshTokenId id):base(id)
        {

        }
        private PasswordResetToken(){}
        
        public bool IsUsed { get; private set; } 

        internal static PasswordResetToken Create(string token, DateTime expiresAt,UserId userId,PasswordRefreshTokenId id)
        {
            return new(id)
            {
                Token = token,
                ExpiresAt = expiresAt,
                IsUsed = false,
                UserId = userId,
            };
        }
        internal void MarkAsUsed()=> IsUsed = true;        
    }
}
