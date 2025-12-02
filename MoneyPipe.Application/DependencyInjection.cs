using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Services;

namespace MoneyPipe.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpContextAccessor();
            
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IEmailTemplateService,EmailTemplateService>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
