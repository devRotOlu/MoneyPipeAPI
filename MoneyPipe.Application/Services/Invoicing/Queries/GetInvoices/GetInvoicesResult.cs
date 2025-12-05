using MoneyPipe.Application.Services.Invoicing.Common;

namespace MoneyPipe.Application.Services.Invoicing.Queries.GetInvoices
{
    public record GetInvoicesResult(IEnumerable<InvoiceResult> Invoices, DateTime? LastTimestamp);
}