using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class InvoiceItem
        {
            public static Error NotFound => Error.NotFound("InvoiceItem.NotFound", description: "No matching invoice was found for the provided identifier.");
            public static Error InvalidItemDescription => Error.Validation(code: "InvoiceItem.InvalidItemDescription", description: "Description is required");
            public static Error InvalidItemPrice => Error.Validation(code: "InvoiceItem.InvalidItemPrice", description: "Total price is required");
            public static Error IdRequired => Error.Validation("InvoiceItem.IdRequired", description: "An invoiceItems ID is required to perform this operation.");
            public static Error UnitPriceRequired => Error.Validation("InvoiceItem.UnitPriceRequired", "Unit price is required when quantity is provided.");
            public static Error TotalPriceConflict => Error.Validation("InvoiceItem.TotalPriceConflict", "Total price must not be set when using quantity and unit price.");
            public static Error MissingPricingInformation => Error.Validation("InvoiceItem.MissingPricing", "Either total price or quantity + unit price must be provided.");
        }
    }
}