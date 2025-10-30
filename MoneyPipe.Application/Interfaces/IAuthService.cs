using ErrorOr;
using MoneyPipe.Application.DTOs;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ErrorOr<Success>> RegisterAsync(RegisterDto dto);
        Task<ErrorOr<AuthResultDTO>> LoginAsync(LoginDTO dto);
        Task<ErrorOr<AuthResultDTO>> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task<(HttpResponseMessage response, string responseBody)> SendEmailForEmailConfirmation(User user, string token, string userName, string? emailConfirmationLink = null);
        Task SendEmailForPasswordReset(User user, string token, string memberFirstName, string? passwordResetLink = null);
        Task<ErrorOr<Success>> ConfirmEmail(string userId,string token);
    }
}
