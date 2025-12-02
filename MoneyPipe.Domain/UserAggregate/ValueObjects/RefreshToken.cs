namespace MoneyPipe.Domain.UserAggregate.ValueObjects
{
    public class RefreshToken : BaseToken
    {
        private RefreshToken(string token, DateTime expiresAt,DateTime? revokedAt,DateTime createdAt,UserId userId)
        :base(token,expiresAt,createdAt,userId)
        {
            RevokedAt = revokedAt;
        }
        
        private RefreshToken(){}

        public DateTime? RevokedAt { get; private set; } 
        internal static RefreshToken Create(string token, DateTime expiresAt,DateTime? revokedAt,DateTime createdAt,UserId userId)
        {
            return new(token,expiresAt,revokedAt,createdAt,userId);
        }
    }
}
