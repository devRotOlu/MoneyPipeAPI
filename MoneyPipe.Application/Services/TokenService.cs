using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyPipe.Application.Common;
using MoneyPipe.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MoneyPipe.Application.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config) { _config = config; }

        private (string accessToken, DateTime accessExp) CreateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessTokenMinutes"]!));
            var expires = DateTime.UtcNow.AddMinutes(15);

            var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
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

        private (string refreshToken, DateTime refreshExp) CreateRefreshToken()
        {
            // create secure random token (use RNG)
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);
            // var expires = DateTime.UtcNow.AddHours(int.Parse(_config["Jwt:RefreshTokenHours"]!));
            var expires = DateTime.UtcNow.AddHours(1);
            return (token, expires);
        }

         private CookieOptions GetAccessTokenCookieOption()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };
        }

        private CookieOptions GetRefreshTokenCookieOption()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
            };
        }

        public (string refreshToken, DateTime refreshTokenExpirationTime) SetTokenInCookies(User user, HttpContext context)
        {
            var (accessToken, accessTokenExpirationTime) = CreateAccessToken(user);
            var (refreshToken, refreshTokenExpirationTime) = CreateRefreshToken();

            var accessTokenOption = GetAccessTokenCookieOption();
            accessTokenOption.Expires = accessTokenExpirationTime;

            context.Response.Cookies.Append(Token.AccessToken, accessToken, accessTokenOption);

            var refreshTokenOption = GetRefreshTokenCookieOption();
            refreshTokenOption.Expires = refreshTokenExpirationTime;

            context.Response.Cookies.Append(Token.RefreshToken, refreshToken, refreshTokenOption);

            return (refreshToken,refreshTokenExpirationTime);
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
        
        public string RetrieveOldRefreshToken(HttpContext context) => context.Request.Cookies[Token.RefreshToken];
    }

}
