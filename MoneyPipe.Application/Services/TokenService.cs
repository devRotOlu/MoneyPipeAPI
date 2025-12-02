using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Domain.UserAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MoneyPipe.Application.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        private readonly IConfiguration _config = config;

        private (string accessToken, DateTime accessExp) CreateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessTokenMinutes"]!));
            var expires = DateTime.UtcNow.AddMinutes(15);

            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role,"User")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        public string GetRefreshToken()
        {
            // create secure random token (use RNG)
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);
            return token;
        }

         private static CookieOptions GetAccessTokenCookieOption()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };
        }

        private static CookieOptions GetRefreshTokenCookieOption()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
            };
        }

        public void SetTokenInCookies(User user,string refreshToken,DateTime refreshTokenExpirationTime,
         HttpContext context)
        {
            var (accessToken, accessTokenExpirationTime) = CreateAccessToken(user);

            var accessTokenOption = GetAccessTokenCookieOption();
            accessTokenOption.Expires = accessTokenExpirationTime;

            context.Response.Cookies.Append(Token.AccessToken, accessToken, accessTokenOption);

            var refreshTokenOption = GetRefreshTokenCookieOption();
            refreshTokenOption.Expires = refreshTokenExpirationTime;

            context.Response.Cookies.Append(Token.RefreshToken, refreshToken, refreshTokenOption);
        }

        public void InvalidateTokenInCookies(HttpContext context)
        {
            
            var accessTokenOption = GetAccessTokenCookieOption();
            accessTokenOption.Expires = DateTime.UtcNow.AddMinutes(-5);
            context.Response.Cookies.Append(Token.AccessToken, "", accessTokenOption);

            var refreshTokenOption = GetRefreshTokenCookieOption();
            refreshTokenOption.Expires = DateTime.UtcNow.AddMinutes(-5);
            context.Response.Cookies.Append(Token.RefreshToken, "", refreshTokenOption);
        }

        private static string GenerateToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            var token = Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');

            return token;
        }
        
        public string RetrieveOldRefreshToken(HttpContext context) => context.Request.Cookies[Token.RefreshToken];

        public string GeneratePasswordResetToken() => GenerateToken();

        public string GenerateEmailConfirmationToken() => GenerateToken();
    }

}
