using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.WalletManagement.Commands.AddVirtualCard
{
    public record AddVirtualCardCommand(Guid WalletId,string Currency):IRequest<ErrorOr<Success>>;
}