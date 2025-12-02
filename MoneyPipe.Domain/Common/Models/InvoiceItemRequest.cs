namespace MoneyPipe.Domain.Models
{
    public record InvoiceItemRequest
    {
        public string Description { get; set; } = null!;
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; } 
        public decimal? TotalPrice { get; set; }
    }
}