namespace MoneyPipe.Domain.Common.Interfaces
{
    public interface IAggregateRoot
    {
        void AddDomainEvent(object eventItem);
        void ClearDomainEvents();
        IReadOnlyCollection<object> DomainEvents {get;}
    }
}