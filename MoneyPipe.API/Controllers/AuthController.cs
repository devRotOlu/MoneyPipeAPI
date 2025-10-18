using Microsoft.AspNetCore.Mvc;
using MoneyPipe.Application.DTOs;
using MoneyPipe.Application.Interfaces;

namespace MoneyPipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) { _auth = auth; }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                await _auth.RegisterAsync(dto);
                return Ok(new { message = "Registered" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            try
            {
                var res = await _auth.LoginAsync(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequestDTO dto)
        {
            try
            {
                var res = await _auth.RefreshAsync(dto.RefreshToken);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshRequestDTO dto)
        {
            await _auth.LogoutAsync(dto.RefreshToken);
            return Ok(new { message = "Logged out" });
        }
    }
}
