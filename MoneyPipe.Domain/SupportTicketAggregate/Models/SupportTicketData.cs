namespace MoneyPipe.Domain.SupportTicketAggregate.Models
{
    public record SupportTicketData(Guid UserId,string? Subject,string Description);
}