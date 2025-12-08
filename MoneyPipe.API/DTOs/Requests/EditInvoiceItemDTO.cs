
namespace MoneyPipe.API.DTOs.Requests
{
    public record EditInvoiceItemDTO:CreateInvoiceItemDTO
    {
        public Guid? InvoiceItemId { get; set; }
    }
}