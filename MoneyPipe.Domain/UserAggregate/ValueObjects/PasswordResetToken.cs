namespace MoneyPipe.Domain.UserAggregate.ValueObjects
{
    public class PasswordResetToken : BaseToken
    {
        private PasswordResetToken(string token, DateTime expiresAt,bool isUsed, DateTime createdAt,UserId userId)
        :base(token,expiresAt,createdAt,userId)
        {
            IsUsed = isUsed;
        }
        private PasswordResetToken(){}
        
        public bool IsUsed { get; private set; } 

        internal static PasswordResetToken Create(string token, DateTime expiresAt,bool isUsed,DateTime createdAt,UserId userId)
        {
            return new(token,expiresAt,isUsed,createdAt,userId);
        }
        
    }
}
