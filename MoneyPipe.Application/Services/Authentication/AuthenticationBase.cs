namespace MoneyPipe.Application.Services.Authentication
{
    public record AuthenticationBase
    {
        public string Email {get;init;} = null!;
        public string Password {get;init;} = null!;
    }
}