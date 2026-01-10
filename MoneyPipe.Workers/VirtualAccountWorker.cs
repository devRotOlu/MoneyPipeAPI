using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Models;
using MoneyPipe.Application.Services;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.WalletAggregate.Model; 
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Workers
{
    public class VirtualAccountWorker(IServiceProvider serviceProvider,ILogger<VirtualAccountWorker> logger) 
    : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<VirtualAccountWorker> _logger = logger;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IEnumerable<BackgroundJob> accountJobs = [];

                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobRepo = scope.ServiceProvider.GetRequiredService<IBackgroundJobReadRepository>();
                    accountJobs = await jobRepo.GetUnCompletedBackgroundJobsAsync(JobTypes.CreateVirtualAccount);
                }
                
                if (!accountJobs.Any())
                {
                    await Task.Delay(5000, stoppingToken);
                    continue;
                }
                await ProcessJobAsync(accountJobs);
            }
        }

        private async Task ProcessJobAsync(IEnumerable<BackgroundJob> jobs)
        {
            foreach (var job in jobs)
            {
                try
                {
                    var payload = job.Payload;

                    var accountJob = AccountJobPayload.Deserialize(payload!);

                    var accountIdResult = VirtualAccountId.CreateUnique(Guid.NewGuid());
                    var accountId = accountIdResult.Value;

                    VirtualAccountResponse response; 
                    using (var _scope = _serviceProvider.CreateScope())
                    {
                        var accountProcessorResolver = _scope.ServiceProvider.GetRequiredService<VirtualAccountProcessorResolver>();
                        var processor = accountProcessorResolver.Resolve(accountJob!.Currency);
                        response = await processor.ProcessVirtualAccount(accountId,accountJob.UserEmail);
                    }
                    
                    var walletId = WalletId.CreateUnique(accountJob.WalletId.Value).Value;

                    var virtualAccountData = new VirtualAccountData(response.BankName,response.AccountId,
                    response.Currency,response.ProviderName,response.AccountNumber);
                
                    using var scope = _serviceProvider.CreateScope();
                    var walletRepo = scope.ServiceProvider.GetRequiredService<IWalletReadRepository>();     
                    var wallet = await walletRepo.GetWallet(walletId);
                    var result = wallet!.AddVirtualAccount(virtualAccountData,accountId);

                    var unitofWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await unitofWork.Wallets.AddVirtualAccount(wallet);

                    job.MarkCompleted(true);
                    await unitofWork.BackgroundJobs.UpdateBackgroundJobAsync(job);
                    await unitofWork.Commit();
                }
                catch(Exception ey) 
                {
                    Console.WriteLine(ey.Message); 
                    Console.WriteLine(ey.InnerException);               
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        job.MarkCompleted(false);
                        await uow.BackgroundJobs.UpdateBackgroundJobAsync(job);
                        await uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        
                        _logger.LogInformation("Error occurred!");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}