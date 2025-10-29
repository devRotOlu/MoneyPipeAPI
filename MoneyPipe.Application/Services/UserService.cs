
using ErrorOr;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.Application.Services
{
        public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<User>> GetByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);

            if (user is null)
                return Errors.User.NotFound;

            return user;
        }

        public async Task<ErrorOr<User>> GetByIdAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null)
                return Errors.User.NotFound;

            return user;
        }

        // public async Task<ErrorOr<Success>> UpdateProfileAsync(Guid userId, string name, string phone)
        // {
        //     var user = await _unitOfWork.Users.GetByIdAsync(userId);
        //         if (user is null)
        //             return Errors.User.NotFound;

        //     user.UpdateProfile(name, phone);
        //     await _userRepository.UpdateAsync(user);

        //     return Result.Success;
        // }
    }
}

