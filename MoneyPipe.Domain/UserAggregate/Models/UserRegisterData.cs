namespace MoneyPipe.Domain.UserAggregate.Models
{
    public record UserRegisterData:UserLoginData
    {
        public string FirstName {get;init;} = null!;
        public string LastName {get;init;} = null!;
        public new string PasswordHash {get;set;} = null!;
        public string EmailConfirmationToken {get;set;} = null!;
        public string EmailMessage {get;set;} = null!;
        public string EmailSubject {get;set;} = null!;
    }
}