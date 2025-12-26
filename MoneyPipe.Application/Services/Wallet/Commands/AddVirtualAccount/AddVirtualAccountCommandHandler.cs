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
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Application.Services.Wallet.Commands.AddVirtualAccount
{
    public class AddVirtualAccountCommandHandler(IWalletReadRepository walletQuery, IUnitOfWork unitofWork, 
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<AddVirtualAccountCommand, ErrorOr<Success>>
    {
        private readonly IWalletReadRepository _walletQuery = walletQuery;
        private readonly IUnitOfWork _unitofWork = unitofWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<ErrorOr<Success>> Handle(AddVirtualAccountCommand request, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var walletIdResult = WalletId.CreateUnique(request.WalletId);

            if (walletIdResult.IsError) return walletIdResult.Errors;

            var walletId = walletIdResult.Value;

            var wallet = await _walletQuery.GetWallet(walletId);

            if (wallet is null) return Errors.Wallet.NotFound;

            var accountJobPayload = new AccountJobPayload(email,request.Currency,walletId);
            var payload = accountJobPayload.Serialize();

            var backgroundJob = BackgroundJob.Create(JobTypes.CreateVirtualAccount);
            backgroundJob.AddPayload(payload);

            await _unitofWork.BackgroundJobs.CreateBackgroundJobAsync(backgroundJob);
            await _unitofWork.Commit();
            
            return Result.Success;
        }
    }
}