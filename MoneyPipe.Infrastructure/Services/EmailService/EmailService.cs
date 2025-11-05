using Microsoft.Extensions.Configuration;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;

namespace MoneyPipe.Infrastructure.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(HttpResponseMessage response, string responseBody)> SendEmail(UserEmailOptions userEmailOptions)
        {
            using var httpClient = new HttpClient();

            // Set API endpoint
            var requestUri = "https://api.resend.com/emails";

            // Set headers
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configuration["ResendConfig:APIToken"]);
            // Compose email payload
            var payload = new
            {
                from = _configuration["ResendConfig:senderAddress"],
                to = userEmailOptions.ToEmails,
                subject = userEmailOptions.Subject,
                html = userEmailOptions.Body
            };

            // Serialize payload to JSON
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");

            // Send POST request
            var response = await httpClient.PostAsync(requestUri, content);

            // Read response body
            var responseBody = await response.Content.ReadAsStringAsync();

            return (response, responseBody);

        }
    }
}
