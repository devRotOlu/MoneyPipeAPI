using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoneyPipe.API.Common.Errors;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.Mapping;
using MoneyPipe.Application.Common;

namespace MoneyPipe.API.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services,ConfigurationManager configuration,IHostEnvironment env)
        {
          
            configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
                
            services.AddSingleton<ProblemDetailsFactory, MoneyPipeProblemDetailsFactory>();

            services.AddControllers().AddNewtonsoftJson(op =>
            {
                op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                op.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });

            services.AddAutoMapper(typeof(APIMappingProfile));

            services.AddAuthorization();

            services.ConfigureAuthentication(configuration);

            services.ConfigureInvalidModelStateResponse();

            services.AddCors(options =>
            {
                options.AddPolicy(CORSPolicy.Policy, policy =>
                {
                    policy.WithOrigins()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();;
                });
            });

            if (env.IsDevelopment()) services.ConfigureSwagger();

            // services.AddHealthChecks()
            //     .AddCheck("self", () => HealthCheckResult.Healthy("App is running"))
            //     .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!, name: "PostgreSQL");

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

                    var response = ApiResponse<object>.Fail(problemDetails.Title!, problemDetails);
                    return new BadRequestObjectResult(response)
                    {
                        ContentTypes = { "application/json" }
                    };
                };
            });

        }

        public static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            //var _ = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   var jwtSettings = configuration.GetSection("Jwt");
                   var key = jwtSettings.GetSection("key").Value;
                   options.IncludeErrorDetails = true;
                   options.RequireHttpsMetadata = true;
                   options.SaveToken = true;

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                       ValidateLifetime = true,
                       ValidAudience = jwtSettings.GetSection("Audience").Value,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                       RequireExpirationTime = true,
                   };

                   // to implement httponly cookie JSON Web Token
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = ctx =>
                      {
                          if (ctx.Request.Cookies.ContainsKey(Token.AccessToken))
                          {
                              ctx.Token = ctx.Request.Cookies[Token.AccessToken];
                          }
                          return Task.CompletedTask;
                      }
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

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MoneyPipeAPI", Version = "v1" });
            });
        }
    }
}
