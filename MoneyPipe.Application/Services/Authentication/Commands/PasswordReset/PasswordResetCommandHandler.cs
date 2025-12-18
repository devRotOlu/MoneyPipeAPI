using ErrorOr;
using MediatR;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.UserAggregate;

namespace MoneyPipe.Application.Services.Authentication.Commands.PasswordReset
{
    public class PasswordResetCommandHandler(IUnitOfWork unitofWork, IUserReadRepository userQuery) : IRequestHandler<PasswordResetCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitofWork = unitofWork;
        private readonly IUserReadRepository _userQuery = userQuery;
        public async Task<ErrorOr<Success>> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
        {

            User? user = await _userQuery.GetUserByIdAsync(request.UserId);

            if (user is null) return Errors.User.NotFound;

            var tokenObj = await _userQuery.GetPasswordResetTokenAsync(request.Token,request.UserId);

            if (tokenObj is null) return Errors.PasswordResetToken.InvalidToken;
            if (tokenObj.ExpiresAt.CompareTo(DateTime.UtcNow) <= 0 || tokenObj.IsUsed) return Errors.PasswordResetToken.Expired;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.MarkUsedPasswordResetToken(tokenObj,passwordHash);

            await _unitofWork.Users.UpdateUserPassword(user);
            await _unitofWork.Users.MarkPasswordResetTokenAsUsedAsync(user);
            await _unitofWork.Commit();

            return Result.Success;
        }
    }
}