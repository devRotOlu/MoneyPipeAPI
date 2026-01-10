using MoneyPipe.Application.Models;
using MoneyPipe.Domain.InvoiceAggregate;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IInvoicePdfGenerator
    {
        byte[] GeneratePdf(Invoice invoice,string? paymentUrl,VirtualAccount? virtualAccount);
    }
}