
namespace MoneyPipe.Workers
{
    public class VirtualCardWorker(IServiceProvider serviceProvider,ILogger<VirtualCardWorker> logger)  : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<VirtualCardWorker> _logger = logger;
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}