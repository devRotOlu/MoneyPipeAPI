using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class KYCProfile
        {
            public static Error InvalidPhoneNumber =>
             Error.Validation(code: "KYCProfile.InvalidPhoneNumber", 
             description: $@"The phone number is invalid. 
             Please enter a valid phone number including country code if required.");
            public static Error MissingRejectionReason => Error.Validation(
                code:"KYCProfile.MissingRejectionReason",
                description:"Enter valid reason for rejection"
            );
        }
    }
}