using MoneyPipe.Domain.InvoiceAggregate;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IInvoicePdfGenerator
    {
        byte[] GeneratePdf(Invoice invoice);
    }
}