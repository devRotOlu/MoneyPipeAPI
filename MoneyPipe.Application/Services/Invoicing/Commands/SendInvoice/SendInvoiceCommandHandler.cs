using ErrorOr;
using MediatR;
using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;


namespace MoneyPipe.Application.Services.Invoicing.Commands.SendInvoice
{
    public class SendInvoiceCommandHandler(IInvoiceReadRepository invoiceQuery, 
    IUnitOfWork unitOfWork,IWalletReadRepository walletQuery) :
     IRequestHandler<SendInvoiceCommand, ErrorOr<Success>>
    {
        private readonly IInvoiceReadRepository _invoiceQuery = invoiceQuery;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IWalletReadRepository _walletQuery = walletQuery;
    
        public async Task<ErrorOr<Success>> Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceQuery.GetByIdAsync(request.InvoiceId);

            if (invoice is null) return Errors.Invoice.NotFound;

            var walletId = WalletId.CreateUnique(request.WalletId).Value;
            var wallet = await _walletQuery.GetWallet(walletId);

            if (wallet is null) return Errors.Wallet.NotFound;

            invoice.MarkSent(request.PaymentMethod);

            var invoiceJobPayload = new InvoiceJobPayload(invoice.Id,walletId,request.PaymentMethod);
            var json = invoiceJobPayload.Serialize();
            var backgroundJob = BackgroundJob.Create(JobTypes.SendInvoice);
            backgroundJob.AddPayload(json);
            
            await _unitOfWork.Invoices.UpdateAsync(invoice);
            await _unitOfWork.BackgroundJobs.CreateBackgroundJobAsync(backgroundJob);
            await _unitOfWork.Commit();

            return Result.Success;
        }
    }
}