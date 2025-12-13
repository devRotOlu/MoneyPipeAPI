using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.UserAggregate.Entities;
using MoneyPipe.Domain.UserAggregate.Events;
using MoneyPipe.Domain.UserAggregate.Models;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate
{
    public sealed class User: AggregateRoot<UserId>
    {
        private User(){}

        private User(UserId id):base(id)
        {
            
        }

        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string DefaultCurrency { get; private set; } = "NGN";
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public string? EmailConfirmationToken { get; private set; } 
        public DateTime? EmailConfirmationExpiry { get; private set; }

        public PasswordResetToken? PasswordResetToken {get;private set;}
        public RefreshToken? RefreshToken {get;private set;}

        // public EmailJob EmailJob {get;private set;}

        // private readonly List<Notification> _notifications = [];
        // public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();


        public static ErrorOr<User> Create(UserRegisterData data)
        {
            var errors = new List<Error>();

            if (string.IsNullOrEmpty(data.Email)) errors.Add(Errors.User.EmailRequired);
            if (string.IsNullOrEmpty(data.FirstName)) errors.Add(Errors.User.FirstRequired);
            if (string.IsNullOrEmpty(data.LastName)) errors.Add(Errors.User.LastRequired);
            if (string.IsNullOrEmpty(data.PasswordHash)) errors.Add(Errors.User.PasswordRequired);

            if (errors.Count > 0) return errors;
            
            var user = new User(UserId.CreateUnique())
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                PasswordHash = data.PasswordHash,
                EmailConfirmationToken = data.EmailConfirmationToken,
                EmailConfirmationExpiry = DateTime.UtcNow.AddHours(24),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return user;
        }

        public void AddUserRegisteredDomainEvent(string? clientURL)
        {
            AddDomainEvent(new UserRegisteredEvent(Id,Email,
            EmailConfirmationToken!,FirstName,clientURL));
        }

        public void AddRequestedPasswordResetDomainEvent(string passwordResetToken,string? resetLink)
        {
            AddDomainEvent(new PasswordResetRequestedEvent(Id,Email,passwordResetToken,FirstName,
            resetLink));
        }

        public void MarkEmailConfirmed()
        {
            EmailConfirmed = true;
            EmailConfirmationToken = null;
            EmailConfirmationExpiry = null;
            UpdatedAt = DateTime.UtcNow; 
        }

        public ErrorOr<Success> SetPasswordResetToken(string token)
        {
            ErrorOr<Success> result= ValidatePasswordResetToken(token);

            if (result.IsError) return result.Errors;

            PasswordResetToken = PasswordResetToken.Create(token, DateTime.UtcNow.AddHours(24),Id);

            return Result.Success;
        }

        private static ErrorOr<Success> ValidatePasswordResetToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return Errors.PasswordResetToken.TokenRequired;
            return Result.Success;
        }

        public ErrorOr<Success> MarkUsedPasswordResetToken(PasswordResetToken tokenObj, string newPasswordHash)
        {
            ErrorOr<Success> result = ValidatePasswordResetToken(tokenObj.Token);

            if (result.IsError) return result.Errors;

            PasswordResetToken = tokenObj;
            PasswordResetToken.MarkAsUsed();

            // new password required
            PasswordHash = newPasswordHash;

            return Result.Success;
        }


        public ErrorOr<DateTime> AddRefreshToken(string token)
        {
            ErrorOr<Success> result = ValidateRefreshToken(token);
            if (result.IsError) return result.Errors;
            var tokenExpirationTime = DateTime.UtcNow.AddHours(1);
            RefreshToken = RefreshToken.Create(token, tokenExpirationTime,Id);
            return tokenExpirationTime;
        }

        private static ErrorOr<Success> ValidateRefreshToken(string token)
        {
            var errors = new List<Error>();
            if (string.IsNullOrEmpty(token)) errors.Add(Errors.RefreshToken.TokenRequired);

            if (errors.Count > 0) return errors;
            return Result.Success;
        }

        public void RevokeRefreshToken(RefreshToken tokenObj)
        {
            RefreshToken = tokenObj;
            RefreshToken.MarkAsRevoked();
        }

        // public Notification AddNotification(NotificationData notification)
        // {
        //     var _notification = Notification.Create(NotificationId.CreateUnique(Guid.NewGuid()),
        //     notification.Title,notification.Message,notification.MetadataJson,notification.Type);

        //     _notifications.Add(_notification);
        //     return _notification;
        // }

        // public ErrorOr<Success> AddEmailJob(string subject,string message,string email)
        // {
        //     var emailJobResult = EmailJob.Create(EmailJobId.CreateUnique(Guid.NewGuid()),
        //         email,message,subject);
        //     if (emailJobResult.IsError) return emailJobResult.Errors;
        //     EmailJob = emailJobResult.Value;

        //     return Result.Success;
        // }

        // public void AddEmailJobHTMLContent(string htmlContent) => EmailJob.AddHTMLContent(htmlContent);
    }
}