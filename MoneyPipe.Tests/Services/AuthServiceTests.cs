using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using MoneyPipe.Application.Services;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.DTOs;
using MoneyPipe.Domain.Entities;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Application.Interfaces.IRepository;

namespace MoneyPipe.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEmailService = new Mock<IEmailService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockMapper = new Mock<IMapper>();

            _authService = new AuthService(
                _mockTokenService.Object,
                _mockUnitOfWork.Object,
                _mockEmailService.Object,
                _mockConfiguration.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnDuplicateEmailError_WhenUserExists()
        {
            // Arrange
            var dto = new RegisterDto { Email = "test@example.com", Password = "password123",FirstName="Ben",LastName="Dan" };
            _mockUnitOfWork.Setup(u => u.Users.GetByEmailAsync(dto.Email))
                .ReturnsAsync(new User { Email = dto.Email });

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.User.DuplicateEmail);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenUserDoesNotExist()
        {
            // Arrange
            var dto = new RegisterDto { Email = "test@example.com", Password = "password123",FirstName="Ben",LastName="Dan" };
            _mockUnitOfWork.Setup(u => u.Users.GetByEmailAsync(dto.Email)).ReturnsAsync((User?)null);
            var user = new User { Email = dto.Email, FirstName = dto.FirstName, LastName = dto.LastName };
            _mockMapper.Setup(m => m.Map<User>(dto)).Returns(user);

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            result.IsError.Should().BeFalse();
            _mockUnitOfWork.Verify(u => u.Users.AddAsync(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnInvalidCredentials_WhenPasswordWrong()
        {
            // Arrange
            var dto = new LoginDTO { Email = "user@example.com", Password = "wrongpass" };
            var user = new User { Email = dto.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct") };
            _mockUnitOfWork.Setup(u => u.Users.GetByEmailAsync(dto.Email)).ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(dto, new DefaultHttpContext());

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.Authentication.InvalidCredentials);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnError_WhenEmailNotConfirmed()
        {
            // Arrange
            var dto = new LoginDTO { Email = "user@example.com", Password = "123456" };
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                EmailConfirmed = false
            };
            _mockUnitOfWork.Setup(u => u.Users.GetByEmailAsync(dto.Email)).ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(dto, new DefaultHttpContext());

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.EmailConfirmation.UnConfirmed);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUserDetails_WhenSuccessful()
        {
            // Arrange
            var dto = new LoginDTO { Email = "user@example.com", Password = "password" };
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                EmailConfirmed = true
            };

            var httpContext = new DefaultHttpContext();

            var userDetailsDto = new UserDetailsDTO { Email = user.Email };

            var refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();
            var userRepoMock = new Mock<IUserRepository>();

            // Setup UnitOfWork repositories
            _mockUnitOfWork.Setup(u => u.Users).Returns(userRepoMock.Object);
            _mockUnitOfWork.Setup(u => u.RefreshTokens).Returns(refreshTokenRepoMock.Object);

            // Setup repository behavior
            userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
            refreshTokenRepoMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>())).ReturnsAsync(1);

            // Setup TokenService and Mapper
            _mockTokenService.Setup(t => t.SetTokenInCookies(user, httpContext))
                .Returns(("refresh-token", DateTime.UtcNow.AddDays(1)));

            _mockMapper.Setup(m => m.Map<UserDetailsDTO>(user)).Returns(userDetailsDto);

            // Act
            var result = await _authService.LoginAsync(dto, httpContext);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Email.Should().Be(dto.Email);
        }

        [Fact]

        public async Task RefreshTokenAsync_ShouldReturnError_WhenTokenIsNull()
        {
            // Arrange
            var refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();

            // Setup unitofwork repository
            _mockUnitOfWork.Setup(u => u.RefreshTokens).Returns(refreshTokenRepoMock.Object);

            var token = "false-token";

            // setup repostory behavior
            refreshTokenRepoMock.Setup(r => r.GetByTokenAsync(token)).ReturnsAsync((RefreshToken?)null);

            // set up httpContext with cookie
            var httpContext = CreateHttpContextWithCookie(token);

            // Act
            var result = await _authService.RefreshTokenAsync(httpContext);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.RefreshToken.InvalidToken);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnError_WhenTokenIsExpired()
        {
            // Arrange

            //  set up refreshToken 
            var refreshToken = new RefreshToken
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(-5),
            };

            var refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();

            // Setup unitofwork repository
            _mockUnitOfWork.Setup(u => u.RefreshTokens).Returns(refreshTokenRepoMock.Object);

             var token = "expired-token";

            // setup repostory behavior
            refreshTokenRepoMock.Setup(r => r.GetByTokenAsync(token)).ReturnsAsync(refreshToken);

            // set up httpContext with cookie
            var httpContext = CreateHttpContextWithCookie(token);

            // Act
            var result = await _authService.RefreshTokenAsync(httpContext);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.RefreshToken.InvalidToken);

        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnError_WhenTokenIsRevoked()
        {
            // Arrange

            //  set up refreshToken 
            var refreshToken = new RefreshToken
            {
                RevokedAt = DateTime.UtcNow.AddMinutes(-5),
            };

            var refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();

            // Setup unitofwork repository
            _mockUnitOfWork.Setup(u => u.RefreshTokens).Returns(refreshTokenRepoMock.Object);

            var token = "revoked-token";

            // setup repostory behavior
            refreshTokenRepoMock.Setup(r => r.GetByTokenAsync(token)).ReturnsAsync(refreshToken);

            // set up httpContext with cookie 
            var httpContext = CreateHttpContextWithCookie(token);

            // Act
            var result = await _authService.RefreshTokenAsync(httpContext);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(Errors.RefreshToken.InvalidToken);
        }
        

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnUserDetails_WhenSuccessful()
        {
            // Arrange
            var token = "valid-token";
            var userId = Guid.NewGuid();
            var oldToken = new RefreshToken { Id = Guid.NewGuid(), ExpiresAt = DateTime.UtcNow.AddMinutes(10), UserId = userId };
            var user = new User { Id = userId, Email = "person@test.com", EmailConfirmed = true };
            var httpContext = CreateHttpContextWithCookie(token);

            _mockUnitOfWork.Setup(u => u.RefreshTokens.GetByTokenAsync(token)).ReturnsAsync(oldToken);
            _mockUnitOfWork.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockTokenService.Setup(t => t.RetrieveOldRefreshToken(httpContext)).Returns(token);
            _mockTokenService.Setup(t => t.SetTokenInCookies(user, httpContext)).Returns(("refresh-token", DateTime.UtcNow.AddDays(1)));
            _mockMapper.Setup(m => m.Map<UserDetailsDTO>(user)).Returns(new UserDetailsDTO { Email = user.Email });
            _mockUnitOfWork.Setup(u => u.RefreshTokens.AddAsync(It.IsAny<RefreshToken>())).ReturnsAsync(1);

            // Act
            var result = await _authService.RefreshTokenAsync(httpContext);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Email.Should().Be(user.Email);

            _mockUnitOfWork.Verify(u => u.RefreshTokens.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.RefreshTokens.RevokeAsync(oldToken.Id), Times.Once);
             _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);

        }
        
        private DefaultHttpContext CreateHttpContextWithCookie(string token)
        {
            var context = new DefaultHttpContext();
            context.Request.Headers["Cookie"] = $"refreshToken={token}";
            return context;
        }

    }
}
