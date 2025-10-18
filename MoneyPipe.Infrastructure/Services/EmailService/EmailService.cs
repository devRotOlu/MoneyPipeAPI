using Microsoft.Extensions.Configuration;
using MoneyPipe.Application.Interfaces.IServices;

namespace MoneyPipe.Infrastructure.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<(HttpResponseMessage response, string responseBody)> SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions, bool isEmailConfirmPage)
        {
            userEmailOptions.Subject = "Confirmation of email Id";

            var templateName = isEmailConfirmPage ? "EmailConfirmationPage" : "EmailTestPage";

            userEmailOptions.Body = UpdatePlaceholders(GetEmailBody(templateName), userEmailOptions.PlaceHolders);

            return await SendEmail(userEmailOptions);
        }

        private string GetEmailBody(string templateName)
        {
            const string templatePath = @"EmailTemplate/{0}.html";

            // Get the full runtime path
            var basePath = AppContext.BaseDirectory;

            var fullPath = Path.Combine(basePath, string.Format(templatePath, templateName));

            var body = File.ReadAllText(fullPath);

            return body;
        }

        private string UpdatePlaceholders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }

        public async Task SendEmailForPasswordReset(UserEmailOptions userEmailOptions, bool isResetPasswordPage)
        {
            userEmailOptions.Subject = "Reset your password";

            var templateName = isResetPasswordPage ? "ForgetPasswordPage" : "ForgetPasswordTest";

            userEmailOptions.Body = UpdatePlaceholders(GetEmailBody(templateName), userEmailOptions.PlaceHolders);

            var (response, responseBody) = await SendEmail(userEmailOptions);

            if (!response.IsSuccessStatusCode)
            {

            }
        }

        private async Task<(HttpResponseMessage response, string responseBody)> SendEmail(UserEmailOptions userEmailOptions)
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
