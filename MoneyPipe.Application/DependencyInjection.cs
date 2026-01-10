using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Mapping;
using MoneyPipe.Application.Services;
using QuestPDF.Infrastructure;

namespace MoneyPipe.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpContextAccessor();
            
            services.AddSingleton<ITokenService,TokenService>();
            services.AddSingleton<IEmailTemplateService,EmailTemplateService>();
            services.AddSingleton<IInvoicePdfGenerator,InvoicePdfGenerator>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(typeof(ApplicationMappingProfile));
            services.AddScoped<VirtualAccountProcessorResolver>();
            services.AddScoped<PaymentCreationResolver>();

            QuestPDF.Settings.License = LicenseType.Community;
            return services;
        }
    }
}
