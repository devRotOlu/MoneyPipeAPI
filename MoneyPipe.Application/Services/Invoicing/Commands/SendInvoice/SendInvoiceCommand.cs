using ErrorOr;
using MediatR;
using MoneyPipe.Domain.InvoiceAggregate.Enums;

namespace MoneyPipe.Application.Services.Invoicing.Commands.SendInvoice
{
    public record SendInvoiceCommand(Guid InvoiceId,Guid WalletId,PaymentMethod PaymentMethod):IRequest<ErrorOr<Success>>;
}