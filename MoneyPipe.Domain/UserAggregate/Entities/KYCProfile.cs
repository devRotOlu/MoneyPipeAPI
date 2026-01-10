using System.Text.RegularExpressions;
using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.UserAggregate.Enums;
using MoneyPipe.Domain.UserAggregate.Models;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Entities
{
    public class KYCProfile:Entity<KYCProfileId>
    {

        private KYCProfile(KYCProfileId id):base(id)
        {
            
        }

        private KYCProfile()
        {
            
        }

        public UserId UserId {get; private set;}
        public string PhoneNumber { get; private set; } = null!;
        public Address Address {get; private set;} 
        public string? RejectionReason {get; private set;} 
        public KYCStatus Status {get; private set;} = KYCStatus.Submitted;
        public DateTime? VerifiedAt { get; private set; }
        private readonly List<KYCDocument> _kycDocuments = [];
        public IReadOnlyList<KYCDocument> Transactions => _kycDocuments.AsReadOnly();
        public KYCDocument? NewKYCDocument {get; private set;} 
        public KYCDocument? VerifiedKYCDocument {get; private set;}

        internal static ErrorOr<KYCProfile> Create(KYCData data,UserId userId)
        {
            List<Error> errors = [];
            var addressResult = Address.Create(data.Street,data.City,data.State,
            data.Country,data.PostalCode,typeof(KYCProfile));
            if (addressResult.IsError) errors.AddRange(addressResult.Errors);
            if (!IsValidPhoneNumber(data.PhoneNumber))
                errors.Add(Errors.KYCProfile.InvalidPhoneNumber);
            
            return errors.Count > 0? errors : new KYCProfile(
                KYCProfileId.CreateUnique(Guid.NewGuid()).Value)
            {
                UserId = userId,
                PhoneNumber = data.PhoneNumber,
                Address = addressResult.Value
            };
        }

        internal ErrorOr<Success> AddDocument(IdentityCategory category,string type,string value,string issuer)
        {
            var documentResult = KYCDocument.Create(category,type,value,issuer,Id,UserId);
            if (documentResult.IsError)
                return documentResult.Errors;
            NewKYCDocument = documentResult.Value;
            _kycDocuments.Add(documentResult.Value);
            return Result.Success;
        }

        internal ErrorOr<Success> ChangeStatus(KYCStatus status,string? rejectionReason)
        {
            if (status.CompareTo(KYCStatus.Rejected) == 0
             && string.IsNullOrEmpty(rejectionReason))
                return Errors.KYCProfile.MissingRejectionReason;
            
            if (status.CompareTo(KYCStatus.Verified) == 0)
                VerifiedAt = DateTime.UtcNow;

            Status = status;
            RejectionReason = rejectionReason;
            return Result.Success;
        }

        internal void VerifyDocument(KYCDocumentId id)
        {
            var match = _kycDocuments.Find(document=> document.Id == id);
            match?.Verify();
            VerifiedKYCDocument = match;
        }

        private static bool IsValidPhoneNumber(string number)
        {
            string pattern = @"^\+?[0-9\s\-\(\)]{7,15}$";
             return Regex.IsMatch(number, pattern);
        }

        /// <summary>
        /// Infrastructure-only method for rehydrating invoice items from persistence.
        /// Do not use in business logic.
        /// </summary>
        public void AddKYCDocuments(IEnumerable<KYCDocument> documents)=> _kycDocuments.AddRange(documents);
    }
}