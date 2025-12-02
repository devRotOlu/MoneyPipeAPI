using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.Authentication.Commands.ConfirmUser
{
    public record ConfirmUserCommand(string UserId,string Token):IRequest<ErrorOr<Success>>;
}