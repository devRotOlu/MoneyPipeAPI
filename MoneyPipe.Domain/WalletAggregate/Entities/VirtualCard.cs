using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.WalletAggregate.Enums;
using MoneyPipe.Domain.WalletAggregate.Model;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Domain.WalletAggregate.Entities
{
    public class VirtualCard:Entity<VirtualCardId>
    {
        public WalletId WalletId { get; private set; } 
        public string CardNumber { get; private set; } 
        public CardExpiryDate CardExpiryDate {get; private set;}
        public string CVC {get;private set;}
        public string Status { get; private set; } = CardStatus.Active.ToString();
        public string Currency {get; private set;} 
        public decimal? CardLimit {get; private set;}
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        private VirtualCard(VirtualCardId id):base(id)
        {
            
        }

        private VirtualCard()
        {
            
        }

        internal static ErrorOr<VirtualCard> Create(VirtualCardData data, WalletId walletId)
        {
            List<Error> errors = [];

            var expiryDateResult = CardExpiryDate.Create(data.ExpiryMonth,data.ExpiryYear);

            if (expiryDateResult.IsError)
                errors.AddRange(expiryDateResult.Errors);
            
            if (!IsValidCardNumber(data.CardNumber))
                errors.Add(Errors.VirtualCard.InvalidCardNumber);
            
            if (!IsValidCVC(data.CVC)) errors.Add(Errors.VirtualCard.InvalidCVV);

            if (IsValidCurrency(data.Currency)) errors.Add(Errors.VirtualCard.InvalidCurrency);

            if (errors.Count != 0) return errors;
           
            return new VirtualCard(VirtualCardId.CreateUnique(Guid.NewGuid()))
            {
                CardNumber = data.CardNumber,
                CardLimit = data.Limit,
                CVC = data.CVC,
                Currency = data.Currency,
                CardExpiryDate = expiryDateResult.Value,
                WalletId = walletId
            };
        }

        internal void SetCardLimit(decimal limit)
        {
            CardLimit = limit;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void RemoveCardLimit()
        {
            CardLimit = null;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void ChangeStatus(CardStatus status)
        {
            Status = status.ToString();
            UpdatedAt = DateTime.UtcNow;
        }

        private static bool IsValidCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency)) return false;
            if (currency.Length != 3) return false;
            if (!currency.All(char.IsLetter)) return false;
            return true;
        }

        private static bool IsValidCVC(string cvc)
        {
            if (string.IsNullOrWhiteSpace(cvc)) return false;
            if (!cvc.All(char.IsDigit)) return false;
            if (cvc.Length < 3 || cvc.Length > 4) return false;
            return true;
        }

         private static bool IsValidCardNumber(string number)
        {
            var sanitized = number.Replace(" ","").Replace("-","");

            if (sanitized.Length < 13 || sanitized.Length > 19)
                return false;
            
            int sum = 0;
            bool alternate = false;

            // Luhn Algorithm
            for (int i = sanitized.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(number[i]))
                    return false;

                int n = number[i] - '0';
                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }
    }
}