using ErrorOr;
using MediatR;
using MoneyPipe.Application.Services.KYCManagement.Common;

namespace MoneyPipe.Application.Services.KYCManagement.Commands.AddDocument
{
    public record AddDocumentCommand : BaseDocumentCommand, IRequest<ErrorOr<Success>>
    {
        public string Type {get; init;} = null!;
        public string Issuer {get; init;} = null!;
    };
}