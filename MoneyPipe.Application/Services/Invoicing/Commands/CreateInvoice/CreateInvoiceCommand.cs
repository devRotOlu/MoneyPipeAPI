using ErrorOr;
using MediatR;
using MoneyPipe.Application.Services.Invoicing.Common;

namespace MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice
{
    public record CreateInvoiceCommand : IRequest<ErrorOr<InvoiceResult>>
    {
        public string Currency {get;init;} = null!;
        public  string? Notes {get;init;}
        public DateTime? DueDate {get;init;}
        public string CustomerEmail {get;init;} = null!;
        public string CustomerName {get;init;} = null!;
        public string? CustomerAddress {get;init;} 
        public IEnumerable<CreateInvoiceItem> InvoiceItems {get;init;} = null!;
    };
    
}