using MoneyPipe.Application.Models;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IEmailService
    {
        Task<(HttpResponseMessage response, string responseBody)> SendEmail(UserEmailOptions userEmailOptions);
    }
}
