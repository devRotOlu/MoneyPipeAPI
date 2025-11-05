using Microsoft.AspNetCore.Http;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface ITokenService
    {
        (string refreshToken, DateTime refreshTokenExpirationTime) SetTokenInCookies(User user, HttpContext context);
        void InvalidateTokenInCookies(HttpContext context);
        string RetrieveOldRefreshToken(HttpContext context);
    }
}