using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class KYCDocument
        {
            public static Error InvalidType=> Error.Validation(
                code:"KYCDocument.InvalidType",
                description:"Provide a valid document type"
            );
            public static Error InvalidTypeValue=> Error.Validation(
                code:"KYCDocument.InvalidTypeValue",
                description:"Provide a valid value for document type"
            );
             public static Error InvalidTypeIssuer=> Error.Validation(
                code:"KYCDocument.InvalidTypeIssuer",
                description:"Provide a valid issuer for document type"
            );
        }
    }
}