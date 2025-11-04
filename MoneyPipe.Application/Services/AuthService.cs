using AutoMapper;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MoneyPipe.Application.DTOs;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Entities;
using System.Security.Cryptography;

namespace MoneyPipe.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        

        public AuthService(TokenService tokenService, IUnitOfWork unitOfWork, IEmailService emailService,IConfiguration configuration,IMapper mapper)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Success>> RegisterAsync(RegisterDto dto)
        {
            var existing = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (existing != null) return Errors.User.DuplicateEmail;

            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = hashed;
            user.EmailConfirmationToken = GenerateToken();
            
            await _unitOfWork.Users.AddAsync(user);
            _unitOfWork.Commit();

            return Result.Success;
        }

        public async Task<ErrorOr<UserDetailsDTO>> LoginAsync(LoginDTO dto,HttpContext httpContext)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Errors.Authentication.InvalidCredentials;

            if (!user.EmailConfirmed) return Errors.EmailConfirmation.UnConfirmed;
            
            var (refreshToken,refreshTokenExpirationTime) = _tokenService.SetTokenInCookies(user,httpContext);

            var accessTokenObj = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = refreshTokenExpirationTime
            };
            await _unitOfWork.RefreshTokens.AddAsync(accessTokenObj);
            _unitOfWork.Commit();

            var userDetails = _mapper.Map<UserDetailsDTO>(user);

            return userDetails;
        }

        public async Task<ErrorOr<UserDetailsDTO>> RefreshAsync(HttpContext context)
        {
            var oldRefreshToken = _tokenService.RetrieveOldRefreshToken(context);
            var stored = await _unitOfWork.RefreshTokens.GetByTokenAsync(oldRefreshToken);

            if (stored == null || stored.RevokedAt != null || stored.ExpiresAt.CompareTo(DateTime.UtcNow) <= 0)
                return Errors.RefreshToken.InvalidToken;

            var user = await _unitOfWork.Users.GetByIdAsync(stored.UserId);
            if (user == null) return Errors.User.NotFound;

            // rotate refresh token: revoke old, create new
            await _unitOfWork.RefreshTokens.RevokeAsync(stored.Id);

            var (newRefreshToken,refreshTokenExpirationTime) = _tokenService.SetTokenInCookies(user,context);

            var newRefreshObj = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = refreshTokenExpirationTime
            };
            
            await _unitOfWork.RefreshTokens.AddAsync(newRefreshObj);
            _unitOfWork.Commit();

            var userDetails = _mapper.Map<UserDetailsDTO>(user);

            return userDetails;
        }

        public async Task LogoutAsync(HttpContext context)
        {
            _tokenService.InvalidateTokenInCookies(context);
            var invalidRefreshToken = _tokenService.RetrieveOldRefreshToken(context);
            var stored = await _unitOfWork.RefreshTokens.GetByTokenAsync(invalidRefreshToken);
            await _unitOfWork.RefreshTokens.RevokeAsync(stored!.Id);
            _unitOfWork.Commit();
        }


        private string GenerateToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            var token = Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');

            return token;
        }

        public async Task<(HttpResponseMessage response, string responseBody)> SendEmailForEmailConfirmationAsync(User user, string token, string userName, string? emailConfirmationLink = null)
        {
            var (userEmailOptions,isEmailConfirmPage) = BuildEmailConfirmationEmail(user, token, userName, emailConfirmationLink);

            userEmailOptions.Subject = "Confirmation of email Id";

            var templateName = isEmailConfirmPage ? "EmailConfirmationPage" : "EmailTestPage";

            userEmailOptions.Body = UpdatePlaceholders(GetEmailBody(templateName), userEmailOptions.PlaceHolders);

            return await _emailService.SendEmail(userEmailOptions);
        }

        private string GetEmailBody(string templateName)
        {
            const string templatePath = @"EmailTemplate/{0}.html";

            // Get the full runtime path
            var basePath = AppContext.BaseDirectory;

            var fullPath = Path.Combine(basePath, string.Format(templatePath, templateName));

            var body = File.ReadAllText(fullPath);

            return body;
        }

        private string UpdatePlaceholders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }

        private async Task<ErrorOr<(string token,User user)>> GenerateTokenForPasswordReset(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);

            if (user is null) return Errors.User.NotFound;

            var token = GenerateToken();

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token!,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            await _unitOfWork.PasswordRestTokens.AddAsync(resetToken);
            _unitOfWork.Commit();

            return (token,user);
        }

        public async Task<ErrorOr<Success>> SendEmailForPasswordResetAsync(string email,string? passwordResetLink = null)
        {
            var tokenResult = await GenerateTokenForPasswordReset(email);

            if (tokenResult.IsError) return tokenResult.Errors;

            var (token,user) = tokenResult.Value;

            var (userEmailOptions, isResetPasswordPage) = BuildForgotPasswordEmail(user,token,user.FirstName,passwordResetLink);

            userEmailOptions.Subject = "Reset your password";

            var templateName = isResetPasswordPage ? "ForgetPasswordPage" : "ForgetPasswordTest";

            userEmailOptions.Body = UpdatePlaceholders(GetEmailBody(templateName), userEmailOptions.PlaceHolders);

            var (response, responseBody) = await _emailService.SendEmail(userEmailOptions);

            if (!response.IsSuccessStatusCode)
            {

            }

            return Result.Success;
        }

        private (UserEmailOptions options,bool isEmailConfirmPage) BuildEmailConfirmationEmail(User user, string token, string userName, string? emailConfirmationLink = null)
        {
            string? appName = _configuration["AppName"];
            string userId = "?id={0}&token={1}";

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>()
                {
                    user.Email!
                },

                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new("{{UserName}}",userName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(emailConfirmationLink + userId,user.Id.ToString(),token)),
                    new KeyValuePair<string, string>("{{AppName}}",appName!),
                    new KeyValuePair<string, string>("{{Token}}",token),
                    new KeyValuePair<string, string>("{{UserId}}",user.Id.ToString())
                }
            };

            var isEmailConfirmPage = !string.IsNullOrEmpty(emailConfirmationLink);

            return (options, isEmailConfirmPage);
        }

        private (UserEmailOptions options,bool isResetPasswordPage) BuildForgotPasswordEmail(User user, string token, string memberFirstName, string? passwordResetLink = null)
        {
            string userIdToken = "?id={0}&token={1}";

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>()
                {
                    user.Email!
                },


                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",memberFirstName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(passwordResetLink+ userIdToken,user.Id.ToString(),token)),
                    new KeyValuePair<string, string>("{{Token}}",token),
                    new KeyValuePair<string, string>("{{UserID}}",user.Id.ToString()),
                }
            };


            var isResetPasswordPage = !string.IsNullOrEmpty(passwordResetLink);

            return (options, isResetPasswordPage);
        }

        public async Task<ErrorOr<Success>> ConfirmEmailAsync(string userId,string token)
        {
            var isUserId = Guid.TryParse(userId, out var _userId);

            var user = new User();

            if (isUserId) user = await _unitOfWork.Users.GetByIdAsync(_userId);

            if (!isUserId || user is null) return Errors.User.NotFound;
            if (user.EmailConfirmationExpiry?.CompareTo(DateTime.UtcNow) < 0) return Errors.EmailConfirmation.TokenExpired;
            if (user.EmailConfirmationToken?.CompareTo(token) != 0) return Errors.EmailConfirmation.TokenMismatch;
            if (user.EmailConfirmed) return Errors.EmailConfirmation.AlreadyConfirmed;

            user.EmailConfirmed = true;
            user.EmailConfirmationExpiry = null;
            user.EmailConfirmationToken = null;

            await _unitOfWork.Users.UpdateAsync(user);
            _unitOfWork.Commit();

            return Result.Success;
            
        }

        public async Task<ErrorOr<Success>> ResetPasswordAsync(PasswordResetDTO dto)
        {
            var isUserId = Guid.TryParse(dto.UserId, out var userId);
            User? user = new();
            if (isUserId) user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (!isUserId || user is null) return Errors.User.NotFound;

            var tokenObj = await _unitOfWork.PasswordRestTokens.GetByTokenAndUserIdAsync(dto.Token, userId);
            if (tokenObj is null) return Errors.PasswordResetToken.InvalidToken;

            if (tokenObj.ExpiresAt < DateTime.UtcNow || tokenObj.IsUsed)
                return Errors.PasswordResetToken.Expired;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.PasswordRestTokens.MarkAsUsedAsync(userId);
            _unitOfWork.Commit();

            return Result.Success;
        }
    }

}
