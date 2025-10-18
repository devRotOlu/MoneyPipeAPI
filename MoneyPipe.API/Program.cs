using MoneyPipe.API;
using MoneyPipe.Application;
using MoneyPipe.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration)
        .AddApplication(builder.Configuration);

    var environment = builder.Environment;

    builder.Services.AddControllers().AddNewtonsoftJson(op =>
    {
        op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        op.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });

    builder.Services.AddAuthorization();

    if (environment.IsDevelopment()) builder.Services.ConfigureSwagger();

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

    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}