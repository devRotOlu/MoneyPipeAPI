using MoneyPipe.Application;
using MoneyPipe.Infrastructure;
using MoneyPipe.Workers;

var builder = Host.CreateApplicationBuilder(args);
{
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddHostedService<InvoiceWorker>();
    builder.Services.AddHostedService<VirtualAccountWorker>();
}

var host = builder.Build();
host.Run();
