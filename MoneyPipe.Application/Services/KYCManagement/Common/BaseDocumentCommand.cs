using MoneyPipe.Domain.UserAggregate.Enums;

namespace MoneyPipe.Application.Services.KYCManagement.Common
{
    public record BaseDocumentCommand
    {
        public IdentityCategory Category {get; init;}
        public string Value {get; init;} = null!;
    }
}