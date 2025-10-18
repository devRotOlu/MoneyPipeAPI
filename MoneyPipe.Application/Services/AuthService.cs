using MoneyPipe.Application.DTOs;
using MoneyPipe.Application.Interfaces.IRepository;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _users;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly TokenService _tokenService;

        public AuthService(IUserRepository users, IRefreshTokenRepository refreshRepo, TokenService tokenService)
        {
            _users = users;
            _refreshRepo = refreshRepo;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            var existing = await _users.GetByEmailAsync(dto.Email);
            if (existing != null) throw new Exception("Email already registered");

            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashed,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
            await _users.AddAsync(user);
        }

        public async Task<AuthResultTDO> LoginAsync(LoginDTO dto)
        {
            var user = await _users.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var (accessToken, accessExp) = _tokenService.CreateAccessToken(user);
            var (refreshToken, refreshExp) = _tokenService.CreateRefreshToken();

            var r = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = refreshExp
            };
            await _refreshRepo.AddAsync(r);

            return new AuthResultTDO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessExp,
                RefreshTokenExpiresAt = refreshExp
            };
        }

        public async Task<AuthResultTDO> RefreshAsync(string refreshToken)
        {
            var stored = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (stored == null || stored.RevokedAt != null || stored.ExpiresAt <= DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            var user = await _users.GetByIdAsync(stored.UserId);
            if (user == null) throw new Exception("User not found");

            // rotate refresh token: revoke old, create new
            await _refreshRepo.RevokeAsync(stored.Id);

            var (accessToken, accessExp) = _tokenService.CreateAccessToken(user);
            var (newRefresh, newRefreshExp) = _tokenService.CreateRefreshToken();

            var newRt = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefresh,
                ExpiresAt = newRefreshExp
            };
            await _refreshRepo.AddAsync(newRt);

            return new AuthResultTDO
            {
                AccessToken = accessToken,
                RefreshToken = newRefresh,
                AccessTokenExpiresAt = accessExp,
                RefreshTokenExpiresAt = newRefreshExp
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var stored = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (stored != null) await _refreshRepo.RevokeAsync(stored.Id);
        }
    }

}
