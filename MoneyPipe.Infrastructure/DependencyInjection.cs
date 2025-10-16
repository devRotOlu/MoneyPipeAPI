using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;
using System.Reflection;


namespace MoneyPipe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.ConfigureDBContext(configuration);
            services.DeployDatabaseChanges(configuration);

            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IMovieRepository, MovieRepository>();

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
