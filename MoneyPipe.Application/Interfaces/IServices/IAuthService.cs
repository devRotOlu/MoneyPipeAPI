using ErrorOr;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.DTOs;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<ErrorOr<Success>> RegisterAsync(RegisterDto dto);
        Task<ErrorOr<UserDetailsDTO>> LoginAsync(LoginDTO dto,HttpContext httpContext);
        Task<ErrorOr<UserDetailsDTO>> RefreshTokenAsync(HttpContext context);
        Task LogoutAsync(HttpContext context);
        Task<(HttpResponseMessage response, string responseBody)> SendEmailForEmailConfirmationAsync(User user, string token, string userName, string? emailConfirmationLink = null);
        Task<ErrorOr<Success>> SendEmailForPasswordResetAsync(string email,string? passwordResetLink = null);
        Task<ErrorOr<Success>> ConfirmEmailAsync(string userId, string token);
        Task<ErrorOr<Success>> ResetPasswordAsync(PasswordResetDTO dto);
    }
}
