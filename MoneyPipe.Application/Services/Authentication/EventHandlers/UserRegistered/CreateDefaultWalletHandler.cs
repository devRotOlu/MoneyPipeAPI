using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Common.Events;
using MoneyPipe.Application.Interfaces;
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
            var result = WalletCreationService.CreateWallet([],userId,"NGN");

            if (result.IsError) Console.WriteLine("");

            var wallet = result.Value;

            using var scope = _serviceProvider.CreateScope();
            var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await _unitOfWork.Wallets.CreateWallet(wallet);
            await _unitOfWork.Commit();
        }
    }
}