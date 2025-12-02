using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Invoice
        {
            public static Error InvoiceItemError => Error.Validation(code: "Invoice.InvoiceItemError", description: "Invoice should contain at least one invoice item.");
            public static Error InvalidCustomerEmail => Error.Validation(code: "Invoice.InvalidCustomerEmail", description: "Customer email is required.");
            public static Error NotFound => Error.NotFound("Invoice.NotFound", description: "No matching invoice was found for the provided identifier.");
            public static Error IdRequired => Error.Validation("Invoice.IdRequired", description: "An invoice ID is required to perform this operation.");
        }
    }
}