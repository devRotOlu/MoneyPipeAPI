using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.Wallet.Commands.AddVirtualAccount
{
    public record AddVirtualAccountCommand(Guid WalletId,string Currency):IRequest<ErrorOr<Success>>;
}