namespace MoneyPipe.Domain.InvoiceAggregate.Models
{
    public record InvoiceItemData
    {
        public string Description { get; set; } = null!;
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; } 
        public decimal? TotalPrice { get; set; }
    }
}