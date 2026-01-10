using System.Security.Claims;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.Services;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Application.Services.WalletManagement.Commands.CreateWallet
{
    public class CreateWalletCommandHandler(IUnitOfWork unitofWork,IMapper mapper,
    IWalletReadRepository walletQuery,IHttpContextAccessor httpContextAccessor) 
    : IRequestHandler<CreateWalletCommand, ErrorOr<WalletResult>>
    {
        private readonly IUnitOfWork _unitofWork = unitofWork;
        private readonly IMapper _mapper = mapper;
        private readonly IWalletReadRepository _walletQuery = walletQuery;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<ErrorOr<WalletResult>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var _userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = UserId.CreateUnique(Guid.Parse(_userId)).Value;

            var wallets = await _walletQuery.GetWallets(userId);

            var result = WalletCreationService.CreateWallet(wallets,userId,request.Currency);

            if (result.IsError) return result.Errors;

            var wallet = result.Value;

            await _unitofWork.Wallets.CreateWallet(wallet);
            await _unitofWork.Commit();

            return _mapper.Map<WalletResult>(wallet);
        }
    }
}