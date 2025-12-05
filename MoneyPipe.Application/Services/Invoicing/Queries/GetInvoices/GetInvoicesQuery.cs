using MediatR;

namespace MoneyPipe.Application.Services.Invoicing.Queries.GetInvoices
{
    public record GetInvoicesQuery(int? PageSize,DateTime? LastTimestamp):
    IRequest<GetInvoicesResult>;
}