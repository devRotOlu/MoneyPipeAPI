using MoneyPipe.API.DTOs.CustomValidations;

namespace MoneyPipe.API.DTOs.Requests
{
    [PricingModelValidation]
    public record EditInvoiceItemDTO:CreateInvoiceItemDTO
    {
        public string? InvoiceItemId { get; set; } 
    }
}