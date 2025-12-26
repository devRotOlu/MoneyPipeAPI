using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.SupportTicketAggregate.Entities;
using MoneyPipe.Domain.SupportTicketAggregate.Enums;
using MoneyPipe.Domain.SupportTicketAggregate.Models;
using MoneyPipe.Domain.SupportTicketAggregate.ValueObjects;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.SupportTicketAggregate
{
    public sealed class SupportTicket : AggregateRoot<SupportTicketId>
    {
        private SupportTicket():base(SupportTicketId.CreateUnique()){}

        private SupportTicket(SupportTicketId id):base(id)
        {
            
        }

        private readonly List<SupportTicketMessage> _messages = [];
        public UserId UserId { get; private set; } = null!;
        public string? Subject { get; private set; } 
        public string Description { get; private set; } = null!;
        public SupportTicketStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }  

        public IReadOnlyCollection<SupportTicketMessage> Messages
            => _messages.AsReadOnly();   

        public static ErrorOr<SupportTicket> Create(SupportTicketData ticket)
        {
            var errors = new List<Error>();

            if (ticket.UserId.CompareTo(Guid.Empty) == 0) errors.Add(Errors.SupportTicket.UserIdRequired);

            if (ticket.Description is null) errors.Add(Errors.SupportTicket.DescriptionRequired);

            var userIdResult = UserId.CreateUnique(ticket.UserId);

            if (userIdResult.IsError) errors.AddRange(userIdResult.Errors);

            if (errors.Count != 0) return errors;

            return new SupportTicket(SupportTicketId.CreateUnique())
            {
                UserId = userIdResult.Value,
                Subject = ticket.Subject,
                Description = ticket.Description!,
                CreatedAt = DateTime.UtcNow
            };
        } 

        public void MarkInProgress()
        {
            if (Status == SupportTicketStatus.Open)
            {
                Status = SupportTicketStatus.InProgress;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void MarkResolved()
        {
            Status = SupportTicketStatus.Resolved;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Close()
        {
            Status = SupportTicketStatus.Closed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddMessage(TicketMessageData data)
        {
            _messages.Add(SupportTicketMessage.Create(data.Message,data.SentAt,data.Sender));
            UpdatedAt = DateTime.UtcNow;
        }
    }
}