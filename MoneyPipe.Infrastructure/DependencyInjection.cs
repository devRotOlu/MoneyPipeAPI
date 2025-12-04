using Dapper;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Infrastructure.Persistence;
using MoneyPipe.Infrastructure.Persistence.Configurations.IdTypeHandlers;
using MoneyPipe.Infrastructure.Persistence.Repositories.Reads;
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
                .DeployDatabaseChanges(configuration);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserReadRepository,UserReadRepository>();
            services.AddScoped<IInvoiceReadRepository,InvoiceReadRepository>();

            services.RegisterAllEntityIdTypeHandlers();

            return services;
        }

        private static IServiceCollection ConfigureDBContext(this IServiceCollection services, ConfigurationManager configuration)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddScoped<IDbConnection>(sp =>
            {
                var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
                connection.Open();
                return connection;
            });

            services.AddScoped(sp =>
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

        public static IServiceCollection RegisterAllEntityIdTypeHandlers(this IServiceCollection services)
        {
            // Find all non-abstract types that inherit from EntityIdTypeHandler<,>
            var handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract &&
                            t.BaseType != null &&
                            t.BaseType.IsGenericType &&
                            t.BaseType.GetGenericTypeDefinition() == typeof(EntityIdTypeHandler<,>))
                .ToList();

            // Find the generic AddTypeHandler<T>(ITypeHandler) method once
            var addHandlerMethod = typeof(SqlMapper)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    m.Name == nameof(SqlMapper.AddTypeHandler) &&
                    m.IsGenericMethod &&
                    m.GetParameters().Length == 1 &&
                    typeof(SqlMapper.ITypeHandler).IsAssignableFrom(m.GetParameters()[0].ParameterType)) ?? throw new InvalidOperationException("Could not find SqlMapper.AddTypeHandler<T>(ITypeHandler) method. Check Dapper version.");
            foreach (var type in handlerTypes)
            {
                // Create an instance of the handler
                var handlerInstance = Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Could not create instance of {type.FullName}. Ensure it has a public parameterless constructor.");

                // Get the strong type (first generic argument)
                var strongType = type.BaseType!.GetGenericArguments()[0];

                // Make the generic AddTypeHandler<T> method for this strong type
                var genericMethod = addHandlerMethod.MakeGenericMethod(strongType);

                // Register the handler with Dapper
                genericMethod.Invoke(null, [handlerInstance]);
            }

            return services;
        }
    }
}
