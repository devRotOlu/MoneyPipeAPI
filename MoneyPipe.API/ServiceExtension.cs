using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using MoneyPipe.API.Common.Errors;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.Configurations;

namespace MoneyPipe.API
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddSingleton<ProblemDetailsFactory, MoneyPipeProblemDetailsFactory>();

            services.AddControllers().AddNewtonsoftJson(op =>
            {
                op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                op.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            services.AddAutoMapper(typeof(MapperInitializer));

            services.AddAuthorization();

            services.ConfigureInvalidModelStateResponse();

            using var serviceProvider = services.BuildServiceProvider();

            var env = serviceProvider.GetRequiredService<IHostEnvironment>();

            if (env.IsDevelopment()) services.ConfigureSwagger();

            return services;
        }

        public static void ConfigureInvalidModelStateResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var factory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                    var problemDetails = factory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

                    var response = ApiResponse<object>.Fail(problemDetails.Title, problemDetails);
                    return new BadRequestObjectResult(response)
                    {
                        ContentTypes = { "application/json" }
                    };
                };
            });

        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization bearer using the Bearer scheme. Enter 'Bearer' [space] and then enter your token in the text input below. Example: 'Bearer 1234abcde' ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    //    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                                Scheme = "Oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                        },
                      new List<string>()
                     }
                });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyShopAPI", Version = "v1" });
            });
        }
    }
}
