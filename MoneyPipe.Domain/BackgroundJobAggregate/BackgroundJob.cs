using System.Text.Json;
using MoneyPipe.Domain.BackgroundJobAggregate.ValueObjects;
using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.BackgroundJobAggregate
{
    public class BackgroundJob:AggregateRoot<BackgroundJobId>
    {
        public string Type { get; private set; } = null!;
        public bool IsCompleted { get; private set; } = false;
        public int Attempts { get; private set; } = 0;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }        
        public JsonDocument? Payload {get; private set;} 

        private BackgroundJob(BackgroundJobId id):base(id)
        {
            
        }

        private BackgroundJob()
        {
            
        }

        public static BackgroundJob Create(string type)
        {
            return new(BackgroundJobId.CreateUnique(Guid.NewGuid()))
            {
                Type = type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        } 

        public void MarkCompleted(bool iscompleted)
        {
            IsCompleted = iscompleted;
            Attempts = ++Attempts;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddPayload(JsonDocument payload)=> Payload = payload;
    }

}