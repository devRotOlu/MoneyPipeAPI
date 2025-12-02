namespace MoneyPipe.Domain.UserAggregate.Models
{
    public record UserLoginData
    {
        public string Email {get;init;} = null!;
        public virtual string PasswordHash {get;init;} = null!;
    };
}