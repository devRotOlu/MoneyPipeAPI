using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.Authentication.Commands.PasswordReset
{
    public record PasswordResetCommand(string Token, string UserId, 
    string NewPassword):IRequest<ErrorOr<Success>>;
}