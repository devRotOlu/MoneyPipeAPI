using MoneyPipe.Application.DTOs;

namespace MoneyPipe.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<AuthResultTDO> LoginAsync(LoginDTO dto);
        Task<AuthResultTDO> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
