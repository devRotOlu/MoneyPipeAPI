namespace MoneyPipe.API.DTOs
{
    public class GetInvoiceItemDTO
    {
        public string Id { get; set; }
        public string Description { get; set; } = null!;
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; } 
        public decimal TotalPrice { get; set; }
    }
}