using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.Invoicing.Commands.SendInvoice
{
    public record SendInvoiceCommand(Guid InvoiceId):IRequest<ErrorOr<Success>>;
}