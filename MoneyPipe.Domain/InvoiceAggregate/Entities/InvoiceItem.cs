using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.InvoiceAggregate.Models;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Domain.InvoiceAggregate.Entities
{
    public class InvoiceItem:Entity<InvoiceItemId>
    {
        public InvoiceItem()
        {
            
        }

        public InvoiceItem(InvoiceItemId id):base(id)
        {
            
        }

        public string Description { get; private set; } = string.Empty;
        public int? Quantity { get; private set; }
        public decimal? UnitPrice { get; private set; } 
        public decimal? TotalPrice { get; private set; }

        internal static ErrorOr<InvoiceItem> Create(InvoiceItemData data)
        {
            var validationResult = ValidateData(data);

            if (validationResult.IsError) return validationResult.Errors;

            var invoiceItem = new InvoiceItem(InvoiceItemId.CreateUnique(Guid.NewGuid()));

            MapDataToInvoiceItem(data,invoiceItem);

            return invoiceItem;
        }

        internal static ErrorOr<InvoiceItem> Edit(EditInvoiceItemData data)
        {
            var errors = new List<Error>();

            if (data.Id == Guid.Empty) errors.Add(Errors.InvoiceItem.IdRequired);

            var validationResult = ValidateData(data);

            if (validationResult.IsError) errors.AddRange(validationResult.Errors);

            if (errors.Count != 0) return errors;

            var invoiceItem = new InvoiceItem(InvoiceItemId.CreateUnique(data.Id));
           
            MapDataToInvoiceItem(data, invoiceItem);

            return invoiceItem;
        }

        private static void MapDataToInvoiceItem(InvoiceItemData data,InvoiceItem item)
        {
            item.Description = data.Description;
            item.Quantity = data.Quantity;
            item.UnitPrice = data.UnitPrice;
            item.TotalPrice = ComputeTotalPrice(data.Quantity, data.UnitPrice, data.TotalPrice);
        }

        private static ErrorOr<Success> ValidateData(InvoiceItemData data)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(data.Description)) errors.Add(Errors.InvoiceItem.InvalidItemDescription);

            var hasQuantity = data.Quantity.HasValue;
            var hasUnitPrice = data.UnitPrice.HasValue;
            var hasTotalPrice = data.TotalPrice.HasValue && data.TotalPrice > 0;

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