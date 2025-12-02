using MoneyPipe.Domain.UserAggregate.Enums;

namespace MoneyPipe.Domain.UserAggregate.Models
{
    public record NotificationData(string Message,string Title,string MetadataJson,NotificationType Type);
}