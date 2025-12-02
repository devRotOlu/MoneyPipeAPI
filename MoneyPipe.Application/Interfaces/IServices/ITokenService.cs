using Microsoft.AspNetCore.Http;
using MoneyPipe.Domain.UserAggregate;


namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface ITokenService
    {
        void SetTokenInCookies(User user,string refreshToken,DateTime refreshTokenExpirationTime,
         HttpContext context);
        void InvalidateTokenInCookies(HttpContext context);
        string RetrieveOldRefreshToken(HttpContext context);
        string GeneratePasswordResetToken();
        string GenerateEmailConfirmationToken();
        string GetRefreshToken();
    }
}