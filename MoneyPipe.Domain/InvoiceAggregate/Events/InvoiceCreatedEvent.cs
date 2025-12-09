using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.InvoiceAggregate.Events
{
    public sealed class InvoiceCreatedEvent(Invoice invoice,Guid userId ,string customerEmail) : DomainEvent
    {
        public Invoice Invoice { get; } = invoice;
        public string CustomerEmail { get; } = customerEmail;
        public Guid UserId {get;} = userId;
    }
}