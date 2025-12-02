using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.DTOs;
using MoneyPipe.Application.Services.Authentication.Commands.ConfirmUser;
using MoneyPipe.Application.Services.Authentication.Commands.Login;
using MoneyPipe.Application.Services.Authentication.Commands.Logout;
using MoneyPipe.Application.Services.Authentication.Commands.PasswordReset;
using MoneyPipe.Application.Services.Authentication.Commands.RefreshToken;
using MoneyPipe.Application.Services.Authentication.Commands.Register;
using MoneyPipe.Application.Services.Authentication.Commands.RequestPasswordReset;
using MoneyPipe.Application.Services.Authentication.Common;

namespace MoneyPipe.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(ISender mediatr,IMapper mapper) 
    : APIController
    {
        private readonly ISender _mediatr = mediatr;
        private readonly IMapper _mapper = mapper;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var command = _mapper.Map<RegisterCommand>(dto);

            ErrorOr<Success> authResult = await _mediatr.Send(command);

            if (authResult.IsError) return Problem(authResult.Errors);

            return Ok(ApiResponse<object>.Ok(null,"Registration Successful"));
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var command = _mapper.Map<LoginCommand>(dto);

            ErrorOr<AuthenticationResult> authResult = await _mediatr.Send(command);

            return authResult.Match(
                result =>
                {
                    var userDetails = _mapper.Map<UserDetailsDTO>(result);
                    return Ok(ApiResponse<UserDetailsDTO>.Ok(userDetails, "Logged in Successfully."));
                },
                Problem
            );
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var command = new RefreshTokenCommand();
            ErrorOr<AuthenticationResult> authResult = await _mediatr.Send(command);
            return authResult.Match(
                result =>
                {
                    var userDetails = _mapper.Map<UserDetailsDTO>(result);
                    return Ok(ApiResponse<UserDetailsDTO>.Ok(userDetails));
                },
                Problem
            );
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var command = new LogoutCommand();
            await _mediatr.Send(command);
            return Ok(ApiResponse<object>.Ok(null,"Logged out Successfully!"));
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var command = new ConfirmUserCommand(userId,token);
            var authResult = await _mediatr.Send(command);
            return authResult.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Email Confirmed")),
                Problem
            );
        }

        [AllowAnonymous]
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromQuery] string email)
        {
            var command = new RequestPasswordResetCommand(email);
            ErrorOr<Success> authResult = await _mediatr.Send(command);

            return authResult.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Password reset link sent if email exists.")),
                Problem
            );
        }

        [AllowAnonymous]
        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset(PasswordResetDTO dto)
        {
            var command = _mapper.Map<PasswordResetCommand>(dto);
            ErrorOr<Success> resetResult = await _mediatr.Send(command);

            return resetResult.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Password reset successful.")),
                Problem
            );
        }      
    }
}
