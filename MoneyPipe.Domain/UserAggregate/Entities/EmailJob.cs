using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Entities
{
    public class EmailJob:Entity<EmailJobId>
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

        internal static ErrorOr<EmailJob> Create(EmailJobId id,string email, string message, string subject)
        {
            var errors = new List<Error>();
            if (string.IsNullOrEmpty(message)) errors.Add(Errors.EmailJob.MessageRequired);
            if (string.IsNullOrEmpty(email)) errors.Add(Errors.EmailJob.EmailRequired);
            if (string.IsNullOrEmpty(subject)) errors.Add(Errors.EmailJob.SubjectRequired);

            if (errors.Count > 0) return errors;
        
            return new EmailJob(id)
            {
                Email = email,
                Message = message,
                Subject = subject
            };
        }

        internal void AddHTMLContent(string htmlContent) => HtmlContent = htmlContent;
    }
}