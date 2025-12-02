using ErrorOr;
using MediatR;

namespace MoneyPipe.Application.Services.Authentication.Commands.Register
{
    public record RegisterCommand:AuthenticationBase,IRequest<ErrorOr<Success>>
    {
        public string FirstName {get;init;} = null!;
        public string LastName {get;init;} = null!;
    }
}