using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;

namespace MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice
{
    public record EditInvoiceItem(string? InvoiceItemId):CreateInvoiceItem;
}