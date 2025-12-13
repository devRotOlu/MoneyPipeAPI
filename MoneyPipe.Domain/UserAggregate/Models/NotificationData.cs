using MoneyPipe.Domain.NotificationAggregate.Enums;

namespace MoneyPipe.Domain.UserAggregate.Models
{
    public record NotificationData(string Message,string Title,string MetadataJson,NotificationType Type);
}