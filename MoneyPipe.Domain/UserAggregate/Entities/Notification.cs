using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.UserAggregate.Enums;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Entities
{
    public sealed class Notification : Entity<NotificationId>
    {
        private Notification()
        {
            
        }

        private Notification(NotificationId id) : base(id)
        {
           
        }
        
        public string Title { get; private set; } 
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; } 
        public bool IsRead { get; private set; } = false;
        public DateTime? ReadAt { get; private set; }
        public NotificationType? Type { get; private set; }
        public string? MetadataJson { get; private set; }

        internal static Notification Create(NotificationId id,string title, string message,string metadataJson,NotificationType? type)
        {
            return new(id)
            {
                CreatedAt = DateTime.UtcNow,
                Message = message,
                Title = title,
                MetadataJson = metadataJson,
                Type = type
            };
        }

        internal void MarkAsRead(DateTime readAt)
        {
            IsRead = true;
            ReadAt = readAt;
        }
    }
}