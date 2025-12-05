using MediatR;
using MoneyPipe.Application.Services.Invoicing.Common;

namespace MoneyPipe.Application.Services.Invoicing.Queries.GetInvoice
{
    public record GetInvoiceQuery(Guid InvoiceId):IRequest<InvoiceResult?>;
}