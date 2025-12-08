namespace MoneyPipe.Application.Services.Authentication.Common
{
    public record AuthenticationResult(string FirstName, string LastName, string Email, Guid Id);
}