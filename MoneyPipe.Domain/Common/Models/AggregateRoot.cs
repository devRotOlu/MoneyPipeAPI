namespace MoneyPipe.Domain.Common.Models
{
    public abstract class AggregateRoot<TId> : Entity<TId> where TId: notnull
    {
        protected AggregateRoot(TId id):base(id)
        {
            
        }
        protected AggregateRoot():base()
        {
            
        }

        private readonly List<object> _domainEvents = [];

        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(object eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }

}