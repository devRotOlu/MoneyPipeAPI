namespace MoneyPipe.API.DTOs.Responses
{
    public record GetInvoicesDTO(IEnumerable<GetInvoiceBase> Invoices,DateTime LastTimestamp);
}