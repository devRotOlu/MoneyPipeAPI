using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class VirtualAccount
        {
            public static Error MissingBankNameError => Error.Validation("VirtualAccount.MissingBankNameError", "Bank name is required to create an account.");
            public static Error MissingAccountNameError => Error.Validation("VirtualAccount.MissingAccountNameError", "Account name is required to create an account.");
            public static Error MissingCurrencyError => Error.Validation("VirtualAccount.MissingCurrencyError", "Currency is required to create an account.");
        }
    }
}