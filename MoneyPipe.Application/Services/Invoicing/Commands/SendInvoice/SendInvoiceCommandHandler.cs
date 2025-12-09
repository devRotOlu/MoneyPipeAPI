using ErrorOr;
using MediatR;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services.Invoicing.Notifications;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.InvoiceAggregate.Events;

namespace MoneyPipe.Application.Services.Invoicing.Commands.SendInvoice
{
    public class SendInvoiceCommandHandler(IInvoiceReadRepository invoiceQuery, IUnitOfWork unitOfWork, IPublisher mediatr) :
     IRequestHandler<SendInvoiceCommand, ErrorOr<Success>>
    {
        private readonly IInvoiceReadRepository _invoiceQuery = invoiceQuery;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPublisher _mediatr = mediatr;

        public async Task<ErrorOr<Success>> Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceQuery.GetByIdAsync(request.InvoiceId);

            if (invoice is null) return Errors.Invoice.NotFound;

            invoice.MarkSent();

            await _unitOfWork.Invoices.UpdateAsync(invoice);
            _unitOfWork.Commit();

            foreach (var _event in invoice.DomainEvents)
                await _mediatr.Publish(new InvoiceCreatedNotification((InvoiceCreatedEvent)_event), cancellationToken);

            invoice.ClearDomainEvents();

            return Result.Success;
        }
    }
}