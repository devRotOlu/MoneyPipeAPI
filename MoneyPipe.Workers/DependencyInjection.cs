using System.Data;
using System.Reflection;
using Dapper;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services;
using MoneyPipe.Infrastructure.Persistence;
using MoneyPipe.Infrastructure.Persistence.Configurations.IdTypeHandlers;
using MoneyPipe.Infrastructure.Persistence.Repositories.Reads;
using MoneyPipe.Infrastructure.Storage;
using Npgsql;

namespace MoneyPipe.Workers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWorker(this IServiceCollection services,ConfigurationManager configuration)
        {
            services.AddHostedService<InvoiceWorker>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IBackgroundJobReadRepository,BackgroundJobReadRepository>();
            services.AddScoped<IInvoiceReadRepository,InvoiceReadRepository>();
            services.AddSingleton<IInvoicePdfGenerator,InvoicePdfGenerator>();
            services.AddSingleton<ICloudinaryService,CloudinaryService>();

            services.ConfigureDBConection(configuration);
            services.RegisterAllEntityIdTypeHandlers();
            
            return services;
        }

        private static IServiceCollection ConfigureDBConection(this IServiceCollection services, ConfigurationManager configuration)
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

        public static IServiceCollection RegisterAllEntityIdTypeHandlers(this IServiceCollection services)
        {
            var assembly = Assembly.Load("MoneyPipe.Infrastructure");
            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract &&
                            t.BaseType != null &&
                            t.BaseType.IsGenericType &&
                            t.BaseType.GetGenericTypeDefinition() == typeof(EntityIdTypeHandler<,>))
                .ToList();

            var addHandlerMethod = typeof(SqlMapper)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    m.Name == nameof(SqlMapper.AddTypeHandler) &&
                    m.IsGenericMethod &&
                    m.GetParameters().Length == 1 &&
                    typeof(SqlMapper.ITypeHandler).IsAssignableFrom(m.GetParameters()[0].ParameterType)) ?? throw new InvalidOperationException("Could not find SqlMapper.AddTypeHandler<T>(ITypeHandler) method. Check Dapper version.");
            foreach (var type in handlerTypes)
            {
                var handlerInstance = Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Could not create instance of {type.FullName}. Ensure it has a public parameterless constructor.");

                var strongType = type.BaseType!.GetGenericArguments()[0];

                var genericMethod = addHandlerMethod.MakeGenericMethod(strongType);

                genericMethod.Invoke(null, [handlerInstance]);
            }

            return services;
        }
    }
}