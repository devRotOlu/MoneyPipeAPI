using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MoneyPipe.API.Controllers;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Domain.Entities;
using ErrorOr;
using MoneyPipe.API.Common.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MoneyPipe.Tests.Helpers;
using MoneyPipe.API.DTOs;

namespace MoneyPipe.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _userServiceMock = new Mock<IUserService>();
            _configMock = new Mock<IConfiguration>();

            _controller = new AuthController(
                _authServiceMock.Object,
                _configMock.Object,
                _userServiceMock.Object
            );

            // Setup a minimal HttpContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = new ServiceCollection()
                    .AddSingleton<ProblemDetailsFactory, TestProblemDetailsFactory>()
                    .BuildServiceProvider()
            };

            // Needed for Request context (e.g. Request.GetClientURL)
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        // -------------------------
        // ✅ Register
        // -------------------------
        [Fact]
        public async Task Register_ShouldReturnOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var dto = new RegisterDto { Email = "test@example.com", Password = "password123",FirstName="Ben",LastName="Dan" };

            _authServiceMock
                .Setup(s => s.RegisterAsync(dto))
                .ReturnsAsync(ErrorOr.ErrorOrFactory.From(new Success()));

            var fakeUser = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                FirstName = "Test",
                EmailConfirmationToken = "12345"
            };

            _userServiceMock
                .Setup(s => s.GetByEmailAsync(dto.Email))
                .ReturnsAsync(fakeUser);

            _authServiceMock
                .Setup(s => s.SendEmailForEmailConfirmationAsync(fakeUser, fakeUser.EmailConfirmationToken!, fakeUser.FirstName, It.IsAny<string>()))
                .ReturnsAsync((new HttpResponseMessage(System.Net.HttpStatusCode.OK), "Email sent"));

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Registration Successful", response.Message);
        }

        // -------------------------
        // ✅ Login
        // -------------------------
        [Fact]
        public async Task Login_ShouldReturnOk_WhenLoginSucceeds()
        {
            // Arrange
            var dto = new LoginDTO { Email = "test@example.com", Password = "password123" };

            var userDetails = new UserDetailsDTO
            {
                Id = Guid.NewGuid().ToString(),
                Email = dto.Email,
                FirstName = "Test"
            };

            _authServiceMock
                .Setup(s => s.LoginAsync(dto, It.IsAny<HttpContext>()))
                .ReturnsAsync(userDetails);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<UserDetailsDTO>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Logged in Successfully.", response.Message);
        }

        [Fact]
        public async Task Login_ShouldReturnProblem_WhenLoginFails()
        {
            // Arrange
            var dto = new LoginDTO { Email = "wrong@example.com", Password = "badpass" };

            var errors = new List<Error> { Error.Validation("InvalidCredentials", "Invalid email or password.") };

            _authServiceMock
                .Setup(s => s.LoginAsync(dto, It.IsAny<HttpContext>()))
                .ReturnsAsync(errors);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var problem = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, problem.StatusCode); // default ProblemDetails code
        }

        // -------------------------
        // ✅ Confirm Email
        // -------------------------
        [Fact]
        public async Task ConfirmEmail_ShouldReturnOk_WhenConfirmed()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var token = "test-token";

            _authServiceMock
                .Setup(s => s.ConfirmEmailAsync(userId, token))
                .ReturnsAsync(ErrorOr.ErrorOrFactory.From(new Success()));

            // Act
            var result = await _controller.ConfirmEmail(userId, token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Email Confirmed", response.Message);
        }

        // -------------------------
        // ✅ Password Reset
        // -------------------------
        [Fact]
        public async Task PasswordReset_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var dto = new PasswordResetDTO
            {
                UserId = "123ggdgdgdg",
                Token = "token123",
                NewPassword = "NewPass123!",
                ConfirmedPassword = "NewPass123!"
            };

            _authServiceMock
                .Setup(s => s.ResetPasswordAsync(dto))
                .ReturnsAsync(ErrorOr.ErrorOrFactory.From(new Success()));

            // Act
            var result = await _controller.PasswordReset(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Password reset successful.", response.Message);
        }
    }
}
