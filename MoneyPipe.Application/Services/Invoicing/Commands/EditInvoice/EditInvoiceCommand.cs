using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;

namespace MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice
{
    public record EditInvoiceCommand : CreateInvoiceCommand
    {
        public Guid InvoiceId {get;init;} 
        public new IEnumerable<EditInvoiceItem> InvoiceItems {get;init;} = null!;
    }  
}