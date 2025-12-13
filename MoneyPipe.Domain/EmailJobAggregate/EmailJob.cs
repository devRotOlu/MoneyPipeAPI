using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.EmailJobAggregate.ValueObjects;

namespace MoneyPipe.Domain.EmailJobAggregate
{
    public class EmailJob:AggregateRoot<EmailJobId>
    {
        private EmailJob(EmailJobId id):base(id)
        {
            
        }
        private EmailJob()
        {
            
        }

        public string Email { get; private set; } = null!;
        public string Subject { get; private set; } = null!;
        public string Message { get; private set; } = null!;
        public string HtmlContent {get;private set;} 
        public string Status { get; private set; } = "pending"; // pending, sent, failed
        public int Attempts { get; private set; } = 0;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public static ErrorOr<EmailJob> Create(string email, string message, string subject)
        {
            var errors = new List<Error>();
            if (string.IsNullOrEmpty(message)) errors.Add(Errors.EmailJob.MessageRequired);
            if (string.IsNullOrEmpty(email)) errors.Add(Errors.EmailJob.EmailRequired);
            if (string.IsNullOrEmpty(subject)) errors.Add(Errors.EmailJob.SubjectRequired);

            if (errors.Count > 0) return errors;
        
            return new EmailJob(EmailJobId.CreateUnique(Guid.NewGuid()))
            {
                Email = email,
                Message = message,
                Subject = subject
            };
        }

        public void AddHTMLContent(string htmlContent) => HtmlContent = htmlContent;
    }
}