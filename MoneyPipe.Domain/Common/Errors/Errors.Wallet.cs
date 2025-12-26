using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Wallet
        {
            public static Error NotFound => Error.NotFound(code: "Wallet.NotFound", description: "No matching Wallet was found for the provided identifier.");
            public static Error InsufficientFundsError => Error.Conflict("Wallet.InsufficientFundsError", "Transaction failed: insufficient funds in wallet.");
            public static Error InvalidCreditAmountError => Error.Validation("Wallet.InvalidCreditAmountError", "Invalid transaction: cannot credit zero or negative amount.");
            public static Error MissingCurrencyError => Error.Validation("Wallet.MissingCurrencyError", "Currency is required to create a wallet.");
            public static Error VirtualCardLimitExceededError => Error.Conflict("Wallet.VirtualCardLimitExceededError","Virtual card limit cannot exceed wallet balance.");
            public static Error AlreadyExists => Error.Conflict("Wallet.AlreadyExists","A wallet with the specified currency already exists for this user.");
        }
    }
}