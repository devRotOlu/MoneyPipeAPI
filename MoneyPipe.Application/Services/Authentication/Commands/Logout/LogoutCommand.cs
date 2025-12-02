using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.Authentication.Commands.Logout
{
    public record LogoutCommand():IRequest<Success>;
}