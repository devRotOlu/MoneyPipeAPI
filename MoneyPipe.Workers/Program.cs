using MoneyPipe.Workers;

var builder = Host.CreateApplicationBuilder(args);
{
    builder.Services.AddWorker(builder.Configuration);
}


var host = builder.Build();
host.Run();
