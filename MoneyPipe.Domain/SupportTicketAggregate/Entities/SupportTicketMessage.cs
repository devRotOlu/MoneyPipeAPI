using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.SupportTicketAggregate.ValueObjects;

namespace MoneyPipe.Domain.SupportTicketAggregate.Entities
{
    public sealed class SupportTicketMessage : Entity<SupportTicketMessageId>
    {
        public string Message { get; private set; } = string.Empty;
        public DateTime SentAt { get; private set; }
        public MessageSender Sender {get; private set;} = null!;

        private SupportTicketMessage():base(SupportTicketMessageId.CreateUnique()){}

        private SupportTicketMessage(SupportTicketMessageId id, string message, DateTime sentAt, MessageSender sender) : base(id)
        {
            Message = message;
            SentAt = sentAt;
            Sender = sender;
        }

        public static SupportTicketMessage Create(string message, DateTime sentAt, MessageSender sender)
        {
            return new(SupportTicketMessageId.CreateUnique(),message,sentAt,sender);
        }
    }
}