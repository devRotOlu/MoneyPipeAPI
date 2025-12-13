using ErrorOr;
using MediatR;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.Common.Errors;


namespace MoneyPipe.Application.Services.Invoicing.Commands.SendInvoice
{
    public class SendInvoiceCommandHandler(IInvoiceReadRepository invoiceQuery, 
    IUnitOfWork unitOfWork,IBackgroundJobQueue jobQueue) :
     IRequestHandler<SendInvoiceCommand, ErrorOr<Success>>
    {
        private readonly IInvoiceReadRepository _invoiceQuery = invoiceQuery;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBackgroundJobQueue _jobQueue = jobQueue;
    
        public async Task<ErrorOr<Success>> Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceQuery.GetByIdAsync(request.InvoiceId);

            if (invoice is null) return Errors.Invoice.NotFound;

            invoice.MarkSent();

            await _unitOfWork.Invoices.UpdateAsync(invoice);
            await _jobQueue.EnqueueSendInvoiceAsync(invoice.Id);
            _unitOfWork.Commit();

            return Result.Success;
        }
    }
}