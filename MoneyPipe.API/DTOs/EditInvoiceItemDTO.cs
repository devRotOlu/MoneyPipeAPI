

using MoneyPipe.API.DTOs.CustomValidations;

namespace MoneyPipe.API.DTOs
{
    [PricingModelValidation]
    public record EditInvoiceItemDTO:CreateInvoiceItemDTO
    {
        public string? InvoiceItemId { get; set; } 
    }
}