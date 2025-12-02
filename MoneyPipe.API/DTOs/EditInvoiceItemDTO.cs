

using MoneyPipe.API.DTOs.CustomValidations;

namespace MoneyPipe.API.DTOs
{
    [PricingModelValidation]
    public record EditInvoiceItemDTO:CreateInvoiceItemsDTO
    {
        public string? Id { get; set; } 
    }
}