using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Services.WalletManagement.Commands.AddVirtualCard
{
    public class AddVirtualCardCommandHandler(IHttpContextAccessor httpContextAccessor,
    IWalletReadRepository walletQuery,IUnitOfWork unitofWork) 
    :IRequestHandler<AddVirtualCardCommand, ErrorOr<Success>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IWalletReadRepository _walletQuery = walletQuery;
        private readonly IUnitOfWork _unitofWork = unitofWork;
        public async Task<ErrorOr<Success>> Handle(AddVirtualCardCommand request, CancellationToken cancellationToken)
        {
            var walletIdResult = WalletId.CreateUnique(request.WalletId);

            Wallet? wallet = null;

            WalletId? walletId = null;

            if (!walletIdResult.IsError)
            {
                walletId = walletIdResult.Value;
                wallet = await _walletQuery.GetWallet(walletId);
            }

            if (wallet is null || walletId is null)
            {
                return Errors.Wallet.NotFound;
            }

            var _userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = UserId.CreateUnique(Guid.Parse(_userId)).Value;

            var cardJobPayload = new CardJobPayload(userId,walletId,request.Currency);
            var jsonDocument = cardJobPayload.Serialize();

            var backgroundJob = BackgroundJob.Create(JobTypes.CreateVirtualCard);
            backgroundJob.AddPayload(jsonDocument);

            await _unitofWork.BackgroundJobs.CreateBackgroundJobAsync(backgroundJob);
            await _unitofWork.Commit();
            
            return Result.Success;
        }
    }
}