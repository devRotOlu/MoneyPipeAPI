using AutoMapper;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.Helpers;
using MoneyPipe.Application.DTOs;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : APIController
    {
        private readonly IAuthService _auth;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService auth, IMapper mapper, IConfiguration configuration,IUnitOfWork unitOfWork)
        {
            _auth = auth;
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = _mapper.Map<User>(dto);

            ErrorOr<Success> authResult = await _auth.RegisterAsync(dto);

            if (authResult.IsError)
            {
                return Problem(authResult.Errors);
            }

            try
            {
                var clientURL = Request.GetClientURL(_configuration["EmailConfirmation"]!);

                await _auth.GenerateEmailConfirmationTokenAsyn(user, user.FirstName, clientURL);
            }
            catch { };

            return Ok(ApiResponse<object>.Ok(null,"Registration Successful"));
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
