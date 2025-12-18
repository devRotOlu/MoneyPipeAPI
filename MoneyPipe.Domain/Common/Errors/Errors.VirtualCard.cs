using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class VirtualCard
        {
            public static Error InvalidCVV => Error.Validation("VirtualCard.InvalidCVV","Card verification value (CVV) is required.");
            public static Error InvalidCardNumber => Error.Validation("VirtualCard.InvalidCardNumber","Card number is required.");
            public static Error InvalidCurrency => Error.Validation("VirtualCard.InvalidCurrency","Currency is required.");
            public static Error InvalidExpiryDate => Error.Validation("VirtualCard.InvalidExpiryDate","Provide a valid expiry date.");
            
        }
    }
}