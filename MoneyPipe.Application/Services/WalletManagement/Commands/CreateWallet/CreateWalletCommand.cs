using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.WalletManagement.Commands.CreateWallet
{
    public record CreateWalletCommand(string Currency):IRequest<ErrorOr<WalletResult>>;
}