using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.WalletAggregate.ValueObjects
{
    public sealed class CardExpiryDate : ValueObject
    {
        private CardExpiryDate(int expiryMonth,int expiryYear)
        {
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
        }

        public int ExpiryMonth {get; private set;} 
        public int ExpiryYear {get; private set;} 
        public static ErrorOr<CardExpiryDate> Create(int expiryMonth,int expiryYear)
        {
            var date = new DateOnly();
            var currentYear = date.Year;

            if (expiryMonth < 1 || expiryMonth > 12 || expiryYear.CompareTo(currentYear) <= 0)
                return Errors.VirtualCard.InvalidExpiryDate;

            return new CardExpiryDate(expiryMonth,expiryYear);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ExpiryMonth;
            yield return ExpiryYear;
        }
    }
}