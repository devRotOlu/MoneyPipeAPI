using MediatR;
using MoneyPipe.Application.Common.Events;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Domain.Common.Interfaces;
using MoneyPipe.Infrastructure.Persistence.Repositories.Writes;
using System.Data;


namespace MoneyPipe.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _transaction;
        private readonly List<IAggregateRoot> _aggregates = [];
        private readonly IPublisher _mediator;

        public IUserWriteRepository Users { get; set; }
        public IInvoiceWriteRepository Invoices {get;set;}
        public IEmailJobWriteRepository EmailJobs { get; set; }
        public IBackgroundJobWriteRepository BackgroundJobs { get; set; }
        public IWalletWriteRepository Wallets { get; set; }

        public UnitOfWork(IDbConnection dbConnection,IPublisher mediator)
        {
            _mediator = mediator;
            _dbConnection = dbConnection;
            _transaction = _dbConnection.BeginTransaction();
            Users = new UserWriteRepository(dbConnection,_transaction);
            Invoices = new InvoiceWriteRepository(dbConnection,_transaction);
            EmailJobs = new EmailJobWriteRepository(dbConnection,_transaction);
            BackgroundJobs = new BackgroundJobWriteRepository(dbConnection,_transaction);
            Wallets = new WalletWriteRepository(dbConnection,_transaction);
        }

        public async Task Commit()
        {
            _transaction.Commit();
            _dbConnection.Close();
            var events = _aggregates.SelectMany(a => a.DomainEvents).ToList();
            foreach (var aggregate in _aggregates)
                aggregate.ClearDomainEvents();

            foreach (var evt in events)
            {
                var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(evt.GetType());
                var notification = (INotification)Activator
                .CreateInstance(notificationType, evt)!;
                await _mediator.Publish(notification);
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _dbConnection.Close();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbConnection?.Dispose();
        }

        public async Task RegisterAggregateAsync(IAggregateRoot aggregate) 
        {
            if (!_aggregates.Contains(aggregate))
                _aggregates.Add(aggregate);
            await Task.CompletedTask;
        }
    }
}
