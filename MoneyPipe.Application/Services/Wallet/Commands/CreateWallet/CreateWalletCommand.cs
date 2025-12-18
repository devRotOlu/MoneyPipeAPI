using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.Wallet.Commands.CreateWallet
{
    public record CreateWalletCommand(string Currency):IRequest<ErrorOr<WalletResult>>;
}