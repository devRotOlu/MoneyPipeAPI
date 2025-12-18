using MoneyPipe.Application.Models;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IEmailTemplateService
    {
        UserEmailOptions BuildForgotPasswordEmail(string id, string username, string token,string email, string? passwordResetLink = null);
        UserEmailOptions BuildEmailConfirmationEmail(string id, string username, string token,string email, string? emailConfirmationLink = null);
        UserEmailOptions BuildInvoiceDeliveryEmail(string name,string email,
        string invoiceNumber,string pdfLink);
    }
}