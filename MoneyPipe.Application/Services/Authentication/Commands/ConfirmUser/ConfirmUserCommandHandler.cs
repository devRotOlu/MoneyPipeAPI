using ErrorOr;
using MediatR;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.UserAggregate;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Application.Services.Authentication.Commands.ConfirmUser
{
    public class ConfirmUserCommandHandler(IUnitOfWork unitofWork, IUserReadRepository userQuery) : IRequestHandler<ConfirmUserCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitofWork = unitofWork;
        private readonly IUserReadRepository _userQuery = userQuery;

        public async Task<ErrorOr<Success>> Handle(ConfirmUserCommand request, CancellationToken cancellationToken)
        {
            var userIdResult = UserId.CreateUnique(request.UserId);

            if (userIdResult.IsError) return userIdResult.Errors;

            User? user = await _userQuery.GetUserByIdAsync(userIdResult.Value);

            if (user is null) return Errors.User.NotFound;
            if (user.EmailConfirmationExpiry?.CompareTo(DateTime.UtcNow) < 0) return Errors.EmailConfirmation.TokenExpired;
            if (user.EmailConfirmationToken?.CompareTo(request.Token) != 0) return Errors.EmailConfirmation.TokenMismatch;
            if (user.EmailConfirmed) return Errors.EmailConfirmation.AlreadyConfirmed;

            user.MarkEmailConfirmed();

            await _unitofWork.Users.MarkConfirmedEmail(user);
            await _unitofWork.Commit();

            return Result.Success;
        }
    }
}