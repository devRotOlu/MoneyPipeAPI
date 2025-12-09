using MediatR;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services.Invoicing.Notifications;

namespace MoneyPipe.Application.Services.Invoicing.Commands.SendInvoice
{
    public class InvoiceCreatedEventHandler(IUnitOfWork unitofWork, IUserReadRepository userQuery,
    IInvoicePdfGenerator pdfGenerator,ICloudinaryService cloudinaryService) 
    : INotificationHandler<InvoiceCreatedNotification>
    {
        private readonly IUnitOfWork _unitofWork = unitofWork;
        private readonly IUserReadRepository _userQuery = userQuery;
        private readonly IInvoicePdfGenerator _pdfGenerator = pdfGenerator;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;

        public async Task Handle(InvoiceCreatedNotification notification, CancellationToken cancellationToken)
        {
            var _event = notification.DomainEvent;
            var user = await _userQuery.GetUserByIdAsync(_event.UserId);

            var pdfBytes = _pdfGenerator.GeneratePdf(_event.Invoice);

            var pdfLink = await _cloudinaryService.UploadPdfAsync(pdfBytes,_event.Invoice.InvoiceNumber);

            user!.AddEmailJob($"Your invoice {_event.Invoice.InvoiceNumber}",
            $"Your invoice is ready. Download pdf {pdfLink}",_event.CustomerEmail);

            await _unitofWork.Users.CreateEmailJobAsync(user);
            _unitofWork.Commit();
        }
    }
}