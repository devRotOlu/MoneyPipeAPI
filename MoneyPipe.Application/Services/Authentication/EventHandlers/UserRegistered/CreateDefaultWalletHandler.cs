using System.Text.Json;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Common;
using MoneyPipe.Application.Common.Events;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.Services;
using MoneyPipe.Domain.UserAggregate.Events;

namespace MoneyPipe.Application.Services.Authentication.EventHandlers.UserRegistered
{
    public class CreateDefaultWalletHandler(IServiceProvider serviceProvider) : INotificationHandler<DomainEventNotification<UserRegisteredEvent>>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public async Task Handle(DomainEventNotification<UserRegisteredEvent> notification, CancellationToken cancellationToken)
        {
            var userId = notification.DomainEvent.UserId;
            var email = notification.DomainEvent.Email;
            var result = WalletCreationService.CreateWallet([],userId,"NGN");

            var wallet = result.Value;

            var accountJobPayload = new AccountJobPayload(email,"NGN",wallet.Id);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new WalletIdConverter());
            var payload = JsonDocument.Parse(JsonSerializer.Serialize(accountJobPayload,options));
            
            var backgroundJob = BackgroundJob.Create(JobTypes.CreateVirtualAccount);
            backgroundJob.AddPayload(payload);

            using var scope = _serviceProvider.CreateScope();
            var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await _unitOfWork.Wallets.CreateWallet(wallet);
            await _unitOfWork.BackgroundJobs.CreateBackgroundJobAsync(backgroundJob);
            await _unitOfWork.Commit();
        }
    }
}