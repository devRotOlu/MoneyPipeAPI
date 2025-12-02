using System.Net;
using System.Text;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;

namespace MoneyPipe.Application.Services
{
    public class EmailTemplateService(IConfiguration configuration):IEmailTemplateService
    {
        private readonly IConfiguration _configuration = configuration;

        public UserEmailOptions BuildForgotPasswordEmail(string id, string username, string token,string email, string? passwordResetLink = null)
        {
            string userIdToken = "?id={0}&token={1}";

            UserEmailOptions options = new()
            {
                ToEmail = email, 

                PlaceHolders =
                [
                    new("{{UserName}}",username),
                    new("{{Link}}",string.Format(passwordResetLink+ userIdToken,id,token)),
                    new("{{Token}}",token),
                    new("{{UserID}}",id),
                ],
                Subject = "Reset your password"
            };

            var templateName = !string.IsNullOrEmpty(passwordResetLink) ? "ForgetPasswordPage" : "ForgetPasswordTest";

            options.HtmlContent = UpdatePlaceholders(GetEmailBody(templateName), options.PlaceHolders);

            options.Message = ConvertHtmlToPlainText(options.HtmlContent);

            return options;
        }

        public UserEmailOptions BuildEmailConfirmationEmail(string id, string username, string token,string email, string? emailConfirmationLink = null)
        {
            string? appName = _configuration["AppName"]; 
            string userIdToken = "?id={0}&token={1}";

            UserEmailOptions options = new()
            {
                ToEmail = email,

                PlaceHolders =
                [
                    new("{{UserName}}",username),
                    new("{{Link}}",string.Format(emailConfirmationLink + userIdToken,id,token)),
                    new("{{AppName}}",appName!),
                    new("{{Token}}",token),
                    new("{{UserId}}",id)
                ],
                Subject = "Confirmation of email Id"
            };

            var templateName = !string.IsNullOrEmpty(emailConfirmationLink) ? "EmailConfirmationPage" : "EmailTestPage";

            options.HtmlContent = UpdatePlaceholders(GetEmailBody(templateName), options.PlaceHolders);

            options.Message = ConvertHtmlToPlainText(options.HtmlContent);

            return options;
        }


        private static string UpdatePlaceholders(string text, List<KeyValuePair<string, string>> keyValuePairs)
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

        private static string GetEmailBody(string templateName)
        {
            const string templatePath = @"EmailTemplate/{0}.html";

            var basePath = AppContext.BaseDirectory;

            var fullPath = Path.Combine(basePath, string.Format(templatePath, templateName));

            var body = File.ReadAllText(fullPath);

            return body;
        }

        private static string ConvertHtmlToPlainText(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var sb = new StringBuilder();

            void Walk(HtmlNode node)
            {
                if (node.NodeType == HtmlNodeType.Text)
                {
                    var text = WebUtility.HtmlDecode(node.InnerText);
                    if (!string.IsNullOrWhiteSpace(text))
                        sb.AppendLine(text.Trim());
                }

                foreach (var child in node.ChildNodes)
                    Walk(child);
            }

            Walk(doc.DocumentNode);
            return sb.ToString().Trim();
        }
    }
}