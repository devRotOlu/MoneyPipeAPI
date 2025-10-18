using MoneyPipe.Application.DTOs;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IRepository;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly TokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(TokenService tokenService, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            var existing = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (existing != null) throw new Exception("Email already registered");

            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hashed,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
            await _unitOfWork.Users.AddAsync(user);
            _unitOfWork.CommitAsync();
        }

        public async Task<AuthResultTDO> LoginAsync(LoginDTO dto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
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
            await _unitOfWork.RefreshTokens.AddAsync(r);
            _unitOfWork.CommitAsync();

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
            var stored = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
            if (stored == null || stored.RevokedAt != null || stored.ExpiresAt <= DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            var user = await _unitOfWork.Users.GetByIdAsync(stored.UserId);
            if (user == null) throw new Exception("User not found");

            // rotate refresh token: revoke old, create new
            await _unitOfWork.RefreshTokens.RevokeAsync(stored.Id);

            var (accessToken, accessExp) = _tokenService.CreateAccessToken(user);
            var (newRefresh, newRefreshExp) = _tokenService.CreateRefreshToken();

            var newRt = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefresh,
                ExpiresAt = newRefreshExp
            };
            await _unitOfWork.RefreshTokens.AddAsync(newRt);
            _unitOfWork.CommitAsync();

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
            var stored = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
            if (stored != null)
            {
                await _unitOfWork.RefreshTokens.RevokeAsync(stored.Id);
                _unitOfWork.CommitAsync();
            }
        }
    }

}
