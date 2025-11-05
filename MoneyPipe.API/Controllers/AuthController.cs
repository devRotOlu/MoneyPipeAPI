using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.Helpers;
using MoneyPipe.Application.DTOs;
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

                await _auth.SendEmailForEmailConfirmationAsync(user, user.EmailConfirmationToken!,user.FirstName, clientURL!);
            }
            catch { };

            return Ok(ApiResponse<object>.Ok(null,"Registration Successful"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            ErrorOr<UserDetailsDTO> authResult = await _auth.LoginAsync(dto,HttpContext);
            return authResult.Match(
                result => Ok(ApiResponse<UserDetailsDTO>.Ok(result, "Logged in Successfully.")),
                errors => Problem(errors)
            );
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            ErrorOr<UserDetailsDTO> authResult = await _auth.RefreshTokenAsync(HttpContext);
            return authResult.Match(
                result => Ok(ApiResponse<UserDetailsDTO>.Ok(result)),
                errors => Problem(errors)
            );
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _auth.LogoutAsync(HttpContext);
            return Ok(ApiResponse<object>.Ok(null,"Logged out Successfully!"));
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            ErrorOr<Success> authResult = await _auth.ConfirmEmailAsync(userId, token);
            return authResult.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Email Confirmed")),
                errors => Problem(errors)
            );
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromQuery] string email)
        {
            var clientURL = Request.GetClientURL(_configuration["PasswordResetRoute"]!);

            ErrorOr<Success> emailResult = await _auth.SendEmailForPasswordResetAsync(email, clientURL);

            return emailResult.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Password reset link sent if email exists.")),
                errors => Problem(errors)
            );
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset(PasswordResetDTO dto)
        {
            ErrorOr<Success> resetResult = await _auth.ResetPasswordAsync(dto);

            return resetResult.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Password reset successful.")),
                errors => Problem(errors)
            );
        }      
    }
}
