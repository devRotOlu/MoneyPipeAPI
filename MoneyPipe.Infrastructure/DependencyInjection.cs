using Dapper.FluentMap;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Domain.Entities;
using Npgsql;
using System.Data;
using System.Reflection;


namespace MoneyPipe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.ConfigureDBContext(configuration)
                .DeployDatabaseChanges(configuration)
                .MapEntities();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection MapEntities(this IServiceCollection services)
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new EntityMapper<User>());
                config.AddMap(new EntityMapper<Invoice>());
                config.AddMap(new EntityMapper<Payment>());
                config.AddMap(new EntityMapper<RefreshToken>());
                config.AddMap(new EntityMapper<Transaction>());
                config.AddMap(new EntityMapper<Wallet>());
            });

            return services;
        }

        private static IServiceCollection ConfigureDBContext(this IServiceCollection services, ConfigurationManager configuration)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddScoped<IDbConnection>(sp =>
            {
                var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
                connection.Open();
                return connection;
            });

            services.AddScoped<IDbTransaction>(sp =>
            {
                var connection = sp.GetRequiredService<IDbConnection>();
                return connection.BeginTransaction();
            });

            return services;
        }

        private static IServiceCollection DeployDatabaseChanges(this IServiceCollection services, ConfigurationManager configuration)
        {
            var upgrader = DeployChanges.To
                            .PostgresqlDatabase(configuration.GetConnectionString("DefaultConnection"))
                            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                            .LogToConsole()
                            .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {

            }

            return services;
        }
    }
}
