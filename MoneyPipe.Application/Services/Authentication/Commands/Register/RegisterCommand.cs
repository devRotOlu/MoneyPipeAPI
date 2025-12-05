using ErrorOr;
using MediatR;
using MoneyPipe.Application.Services.Authentication.Common;


namespace MoneyPipe.Application.Services.Authentication.Commands.Register
{
    public record RegisterCommand:AuthenticationBase,IRequest<ErrorOr<Success>>
    {
        public string FirstName {get;init;} = null!;
        public string LastName {get;init;} = null!;
    }
}