using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.Authentication.Commands.RequestPasswordReset
{
    public record RequestPasswordResetCommand(string Email):IRequest<ErrorOr<Success>>;
}