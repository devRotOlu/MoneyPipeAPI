using ErrorOr;
using MediatR;
using MoneyPipe.Application.Services.KYCManagement.Common;

namespace MoneyPipe.Application.Services.KYCManagement.Commands.CompleteProfile
{
    public record CompleteProfileCommand(string PhoneNumber,
    string Street, string City, string State,string Country,string PostalCode,
    BaseDocumentCommand NIN):IRequest<ErrorOr<Success>>;
}