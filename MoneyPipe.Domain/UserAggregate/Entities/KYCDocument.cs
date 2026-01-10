using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.UserAggregate.Enums;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.UserAggregate.Entities
{
    public class KYCDocument:Entity<KYCDocumentId>
    {
        private KYCDocument(KYCDocumentId id):base(id)
        {
            
        }

        private KYCDocument()
        {
            
        }

        public UserId UserId {get; private set;}
        public KYCProfileId KYCProfileId {get;private set;}
        public IdentityCategory Category { get; private set; } 
        public string Type { get; private set; } = null!;   // "NIN", "BVN", "PASSPORT", "RC"
        public string Value { get; private set; } = null!; // encrypted
        public bool IsVerified { get; private set; } 
        public DateTime? VerifiedAt { get; private set; }
        public string Issuer { get; private set; } = null!;

        internal static ErrorOr<KYCDocument> Create(IdentityCategory category,
        string type,string value,string issuer,KYCProfileId profileId,UserId userId)
        {
            List<Error> errors = [];

            if (string.IsNullOrEmpty(type))
                errors.Add(Errors.KYCDocument.InvalidType);
            if (string.IsNullOrEmpty(value))
                errors.Add(Errors.KYCDocument.InvalidTypeValue);
            if (string.IsNullOrEmpty(issuer))
                errors.Add(Errors.KYCDocument.InvalidTypeIssuer);
            
            return errors.Count > 0? errors: new KYCDocument
            (KYCDocumentId.CreateUnique(Guid.NewGuid()).Value)
            {
                Category = category,
                Type = type,
                Value = value,
                Issuer = issuer,
                KYCProfileId = profileId,
                UserId = userId
            };
        }

        internal void Verify()
        {
            IsVerified = true;
            VerifiedAt = DateTime.UtcNow;
        }
    }
}