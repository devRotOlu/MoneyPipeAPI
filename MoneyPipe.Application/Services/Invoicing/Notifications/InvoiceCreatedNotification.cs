using MediatR;
using MoneyPipe.Domain.InvoiceAggregate.Events;

namespace MoneyPipe.Application.Services.Invoicing.Notifications
{
    public sealed class InvoiceCreatedNotification(InvoiceCreatedEvent domainEvent) : INotification
    {
        public InvoiceCreatedEvent DomainEvent { get; } = domainEvent;
    }
}