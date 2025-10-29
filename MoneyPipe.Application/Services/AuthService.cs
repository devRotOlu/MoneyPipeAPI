using AutoMapper;
using ErrorOr;
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
            _unitOfWork.CommitAsync();

            return Result.Success;
        }

        public async Task<ErrorOr<AuthResultDTO>> LoginAsync(LoginDTO dto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Errors.Authentication.InvalidCredentials;

            var (accessToken, accessExp) = _tokenService.CreateAccessToken(user);
            var (refreshToken, refreshExp) = _tokenService.CreateRefreshToken();


            var accessTokenObj = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = refreshExp
            };
            await _unitOfWork.RefreshTokens.AddAsync(accessTokenObj);
            _unitOfWork.CommitAsync();

            var authResultDTO = MapToAuthResult(user, accessToken, accessExp, refreshToken, refreshExp);

            return authResultDTO;
        }

        private AuthResultDTO MapToAuthResult(User user, string accessToken, DateTime accessExp, string refreshToken, DateTime refreshExp)
        {
            var authResultDTO = _mapper.Map<AuthResultDTO>(user);
            authResultDTO.AccessToken = accessToken;
            authResultDTO.RefreshToken = refreshToken;
            authResultDTO.AccessTokenExpiresAt = accessExp;
            authResultDTO.RefreshTokenExpiresAt = refreshExp;
            return authResultDTO;
        }

        public async Task<ErrorOr<AuthResultDTO>> RefreshAsync(string refreshToken)
        {
            var stored = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);
            if (stored == null || stored.RevokedAt != null || stored.ExpiresAt <= DateTime.UtcNow)
                return Errors.RefreshToken.InvalidToken;

            var user = await _unitOfWork.Users.GetByIdAsync(stored.UserId);
            if (user == null) return Errors.User.NotFound;

            // rotate refresh token: revoke old, create new
            await _unitOfWork.RefreshTokens.RevokeAsync(stored.Id);

            var (accessToken, accessExp) = _tokenService.CreateAccessToken(user);
            var (newRefresh, newRefreshExp) = _tokenService.CreateRefreshToken();

            var newRefreshObj = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefresh,
                ExpiresAt = newRefreshExp
            };
            
            await _unitOfWork.RefreshTokens.AddAsync(newRefreshObj);
            _unitOfWork.CommitAsync();

            var authResultDTO = MapToAuthResult(user, accessToken, accessExp, newRefresh, newRefreshExp);

            return authResultDTO;
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

        public async Task<(HttpResponseMessage response, string responseBody)> SendEmailForEmailConfirmation(User user, string token, string userName, string? emailConfirmationLink = null)
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

        public async Task SendEmailForPasswordReset(User user, string token, string memberFirstName, string? passwordResetLink = null)
        {
            var (userEmailOptions, isResetPasswordPage) = BuildForgotPasswordEmail(user,token,memberFirstName,passwordResetLink);

            userEmailOptions.Subject = "Reset your password";

            var templateName = isResetPasswordPage ? "ForgetPasswordPage" : "ForgetPasswordTest";

            userEmailOptions.Body = UpdatePlaceholders(GetEmailBody(templateName), userEmailOptions.PlaceHolders);

            var (response, responseBody) = await _emailService.SendEmail(userEmailOptions);

            if (!response.IsSuccessStatusCode)
            {

            }
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
            string userId = "?id={0}&token={1}";

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>()
                {
                    user.Email!
                },

                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",memberFirstName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(passwordResetLink+ userId,user.Id.ToString(),token)),
                    new KeyValuePair<string, string>("{{Token}}",token),
                    new KeyValuePair<string, string>("{{UserID}}",user.Id.ToString()),
                }
            };


            var isResetPasswordPage = !string.IsNullOrEmpty(passwordResetLink);

            return (options, isResetPasswordPage);
        }
    }

}
