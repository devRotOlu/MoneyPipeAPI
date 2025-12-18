using MediatR;
using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Application.Common.Events
{
    public sealed class DomainEventNotification<TDomainEvent>(TDomainEvent domainEvent) : INotification
        where TDomainEvent : DomainEvent
    {
        public TDomainEvent DomainEvent { get; } = domainEvent;
    }
}