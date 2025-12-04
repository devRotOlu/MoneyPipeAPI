using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;

namespace MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice
{
    public record EditInvoiceCommand : CreateInvoiceCommand
    {
        public string InvoiceId {get;init;} = null!;
        public new IEnumerable<EditInvoiceItem> InvoiceItems {get;init;} = null!;
    }  
}