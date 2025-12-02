using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Domain.InvoiceAggregate.Entities
{
    public class InvoiceItem:Entity<InvoiceItemId>
    {
        public InvoiceItem()
        {
            
        }

        public string Description { get; private set; } = string.Empty;
        public int? Quantity { get; private set; }
        public decimal? UnitPrice { get; private set; } 
        public decimal? TotalPrice { get; private set; }
    }
}