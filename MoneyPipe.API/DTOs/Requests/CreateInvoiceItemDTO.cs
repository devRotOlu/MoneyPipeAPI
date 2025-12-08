using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs.Requests
{
    public record CreateInvoiceItemDTO:IValidatableObject
    {
        [Required(ErrorMessage ="This is a required field"), MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; init; } = null!;
        [Range(1, double.MaxValue, ErrorMessage = "Only values greater than 0 are allowed")]
        public int? Quantity { get; init; }
        [Range(1, double.MaxValue, ErrorMessage = "Only values greater than 0 are allowed")]
        public decimal? UnitPrice { get; init; } 
        [Range(1, double.MaxValue, ErrorMessage = "Only values greater than 0 are allowed")]
        public decimal? TotalPrice { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var hasQuantity = Quantity.HasValue;
            var hasUnitPrice = UnitPrice.HasValue;
            var hasTotalPrice = TotalPrice.HasValue && TotalPrice > 0;

            if ((hasQuantity && hasUnitPrice && !hasTotalPrice) ||
                (!hasQuantity && !hasUnitPrice && hasTotalPrice))
            {
                yield break; // valid
            }

            yield return new ValidationResult(
                "Provide either TotalPrice or Quantity + UnitPrice, but not both.",
                [nameof(Quantity), nameof(UnitPrice), nameof(TotalPrice)]);
        }
    }
}