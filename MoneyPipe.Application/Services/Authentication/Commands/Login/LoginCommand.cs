using ErrorOr;
using MediatR;
using MoneyPipe.Application.Services.Authentication.Common;

namespace MoneyPipe.Application.Services.Authentication.Commands.Login
{
    public record LoginCommand:AuthenticationBase,IRequest<ErrorOr<AuthenticationResult>>
    {
        
    }
}