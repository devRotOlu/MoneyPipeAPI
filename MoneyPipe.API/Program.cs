using MoneyPipe.API;
using MoneyPipe.API.Middleware;
using MoneyPipe.Application;
using MoneyPipe.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication(builder.Configuration)
        .AddPresentation();

    //builder.Services.ConfigureAuthentication(builder);
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
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}