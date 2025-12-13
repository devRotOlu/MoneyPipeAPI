using Microsoft.AspNetCore.Mvc.Infrastructure;
using MoneyPipe.API.Common.Http;
using System.Text.Json;

namespace MoneyPipe.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Resolve your custom ProblemDetailsFactory
            var factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            // Create enriched ProblemDetails
            var problemDetails = factory.CreateProblemDetails(
                context,
                statusCode: statusCode,
                title: "An error occurred while processing your request.",
                detail: exception.Message
            );

            // Wrap it in your ApiResponse
            var response = ApiResponse<object>.Fail(problemDetails!.Title!, problemDetails);

            // Serialize and write to response
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            await context.Response.WriteAsync(json);
        }

    }
}
