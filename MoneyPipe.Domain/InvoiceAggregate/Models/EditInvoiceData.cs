
namespace MoneyPipe.Domain.InvoiceAggregate.Models
{
    public record EditInvoiceData:InvoiceData
    { 
        public Guid Id { get; set; }
        public new IEnumerable<EditInvoiceItemData> InvoiceItems { get; set; } = null!;
        //IEnumerable<InvoiceItemData> IInvoiceData.InvoiceItems => InvoiceItems;
    }
}