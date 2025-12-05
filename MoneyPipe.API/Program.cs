using MoneyPipe.API.Common.Http;
using MoneyPipe.API.Configurations;
using MoneyPipe.API.Middleware;
using MoneyPipe.Application;
using MoneyPipe.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication(builder.Configuration)
        .AddPresentation(builder.Configuration,builder.Environment);
}

var app = builder.Build();
{

    //Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();

    app.UseWhen(ctx => !ctx.Request.Path.StartsWithSegments("/health"), branch =>
    {
        branch.UseMiddleware<ErrorHandlingMiddleware>();
    });

    app.UseRouting();
    app.UseCors(CORSPolicy.Policy);
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGet("/", () => "Welcome to MoneyPipe!");
    app.MapControllers();

    // app.MapHealthChecks("/health", new HealthCheckOptions
    // {
    //     ResponseWriter = async (context, report) =>
    //     {
    //         context.Response.ContentType = "application/json";
    //         var result = JsonSerializer.Serialize(new
    //         {
    //             status = report.Status.ToString(),
    //             checks = report.Entries.Select(e => new
    //             {
    //                 component = e.Key,
    //                 status = e.Value.Status.ToString(),
    //                 error = e.Value.Exception?.Message
    //             }),
    //             totalDuration = report.TotalDuration.TotalMilliseconds + "ms"
    //         });
    //         await context.Response.WriteAsync(result);
    //     }
    // });

    app.Run();
}