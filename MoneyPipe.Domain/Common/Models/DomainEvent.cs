namespace MoneyPipe.Domain.Common.Models
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

}