using ErrorOr;
using MediatR;
using MoneyPipe.Application.Services.Authentication.Common;

namespace MoneyPipe.Application.Services.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand():IRequest<ErrorOr<AuthenticationResult>>;
}