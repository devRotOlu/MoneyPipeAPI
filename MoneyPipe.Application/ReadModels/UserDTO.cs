namespace MoneyPipe.Application.ReadModels
{
    public record UserDTO(Guid Id,string Email, string PasswordHash, string FirstName, string LastName, 
    string DefaultCurrency, bool EmailConfirmed, string EmailConfirmationToken,
    DateTime EmailConfirmationExpiry);
}