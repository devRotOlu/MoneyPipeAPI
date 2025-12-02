using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Models;

namespace MoneyPipe.Domain.Entities
{
    public class InvoiceItem
    {
        public Guid Id { get; private set; }
        public Guid? InvoiceId { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public int? Quantity { get; private set; }
        public decimal? UnitPrice { get; private set; } 
        public decimal? TotalPrice { get; private set; }

        private InvoiceItem(){}

        internal static ErrorOr<InvoiceItem> Create(InvoiceItemRequest request)
        {
            var validationResult = ValidateRequest(request);

            if (validationResult.IsError) return validationResult.Errors;

            var invoiceItem = new InvoiceItem();

            MapRequestToInvoiceItem(request,invoiceItem);

            return invoiceItem;
        }

        internal static ErrorOr<InvoiceItem> Edit(EditInvoiceItemRequest request)
        {
            var errors = new List<Error>();

            if (request.Id == Guid.Empty) errors.Add(Errors.InvoiceItem.IdRequired);

            var validationResult = ValidateRequest(request);

            if (validationResult.IsError) errors.AddRange(validationResult.Errors);

            if (errors.Count != 0) return errors;

            var invoiceItem = new InvoiceItem
            {
                Id = request.Id
            };

            MapRequestToInvoiceItem(request, invoiceItem);

            return invoiceItem;
        }

        private static void MapRequestToInvoiceItem(InvoiceItemRequest request,InvoiceItem item)
        {
            item.Description = request.Description;
            item.Quantity = request.Quantity;
            item.UnitPrice = request.UnitPrice;
            item.TotalPrice = ComputeTotalPrice(request.Quantity, request.UnitPrice, request.TotalPrice);
        }

        private static ErrorOr<Success> ValidateRequest(InvoiceItemRequest request)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(request.Description)) errors.Add(Errors.InvoiceItem.InvalidItemDescription);

            var hasQuantity = request.Quantity.HasValue;
            var hasUnitPrice = request.UnitPrice.HasValue;
            var hasTotalPrice = request.TotalPrice.HasValue && request.TotalPrice > 0;

            // Case 1: Product-style pricing
            if (hasQuantity && hasUnitPrice && !hasTotalPrice)
                return Result.Success;

            // Case 2: Flat-rate service
            if (!hasQuantity && !hasUnitPrice && hasTotalPrice)
                return Result.Success;

            // Invalid combinations
            if (hasQuantity && !hasUnitPrice)
                errors.Add(Errors.InvoiceItem.UnitPriceRequired);

            if (hasQuantity && hasUnitPrice && hasTotalPrice)
                errors.Add(Errors.InvoiceItem.TotalPriceConflict);

            if (!hasQuantity && !hasUnitPrice && !hasTotalPrice)
                errors.Add(Errors.InvoiceItem.MissingPricingInformation);

            if (errors.Count != 0) return errors;

            return Result.Success;
        }
        
        public static decimal? ComputeTotalPrice(int? quantity,decimal? unitPrice,decimal? totalPrice)
        {
            return quantity is not null && unitPrice is not null? quantity * unitPrice
            :totalPrice;
        }
    }
}