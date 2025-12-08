namespace MoneyPipe.API.DTOs.Responses
{
    public class GetInvoiceItemDTO
    {
        public Guid Id { get; set; } 
        public string Description { get; set; } = null!;
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; } 
        public decimal TotalPrice { get; set; }
    }
}