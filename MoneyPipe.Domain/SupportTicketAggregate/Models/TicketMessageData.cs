using MoneyPipe.Domain.SupportTicketAggregate.ValueObjects;

namespace MoneyPipe.Domain.SupportTicketAggregate.Models
{
    public record TicketMessageData(string Message,DateTime SentAt,MessageSender Sender);
}