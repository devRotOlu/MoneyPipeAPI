using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.Helpers;
using MoneyPipe.Application.DTOs;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : APIController
    {
        private readonly IAuthService _auth;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
    
        public AuthController(IAuthService auth, IConfiguration configuration,IUserService userService)
        {
            _auth = auth;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {

            ErrorOr<Success> authResult = await _auth.RegisterAsync(dto);

            if (authResult.IsError)
            {
                return Problem(authResult.Errors);
            }

            try
            {
                var clientURL = Request.GetClientURL(_configuration["EmailConfirmation"]!);

                ErrorOr<User> userResult = await _userService.GetByEmailAsync(dto.Email);

                var user = userResult.Value;

                await _auth.SendEmailForEmailConfirmation(user, user.FirstName, clientURL!);
            }
            catch { };

            return Ok(ApiResponse<object>.Ok(null,"Registration Successful"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            ErrorOr<AuthResultDTO> authResult = await _auth.LoginAsync(dto);
            return authResult.Match(
                result => Ok(ApiResponse<AuthResultDTO>.Ok(result, "Logged in Successfully.")),
                errors => Problem(errors)
            );
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequestDTO dto)
        {
            ErrorOr<AuthResultDTO> authResult = await _auth.RefreshAsync(dto.RefreshToken);
            return authResult.Match(
                result => Ok(ApiResponse<AuthResultDTO>.Ok(result)),
                errors => Problem(errors)
            );
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshRequestDTO dto)
        {
            await _auth.LogoutAsync(dto.RefreshToken);
            return Ok(ApiResponse<object>.Ok("Logged out Successfully!"));
        }
    }
}
