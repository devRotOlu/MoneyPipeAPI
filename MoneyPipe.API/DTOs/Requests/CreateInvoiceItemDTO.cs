using System.ComponentModel.DataAnnotations;
using MoneyPipe.API.DTOs.CustomValidations;

namespace MoneyPipe.API.DTOs.Requests
{
    [PricingModelValidation]
    public record CreateInvoiceItemDTO
    {
        [Required(ErrorMessage ="This is a required field"), MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; init; } = null!;
        [Range(1, double.MaxValue, ErrorMessage = "Only values greater than 0 are allowed")]
        public int? Quantity { get; init; }
        [Range(1, double.MaxValue, ErrorMessage = "Only values greater than 0 are allowed")]
        public decimal? UnitPrice { get; init; } 
        [Range(1, double.MaxValue, ErrorMessage = "Only values greater than 0 are allowed")]
        public decimal? TotalPrice { get; init; }
    }
}