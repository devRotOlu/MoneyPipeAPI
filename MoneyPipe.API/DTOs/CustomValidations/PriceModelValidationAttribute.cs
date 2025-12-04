using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs.CustomValidations
{
        public class PricingModelValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var request = (CreateInvoiceItemDTO)validationContext.ObjectInstance;

            var hasQuantity = request.Quantity.HasValue;
            var hasUnitPrice = request.UnitPrice.HasValue;
            var hasTotalPrice = request.TotalPrice > 0;

            if (hasQuantity && hasUnitPrice && !hasTotalPrice)
                return ValidationResult.Success;

            if (!hasQuantity && !hasUnitPrice && hasTotalPrice)
                return ValidationResult.Success;

            return new ValidationResult("Provide either TotalPrice or Quantity + UnitPrice, but not both.");
        }
    }

}