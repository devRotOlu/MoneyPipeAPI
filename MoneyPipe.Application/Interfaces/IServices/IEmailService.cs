namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IEmailService
    {
        Task<(HttpResponseMessage response, string responseBody)> SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions, bool isEmailConfirmPage);
        Task SendEmailForPasswordReset(UserEmailOptions userEmailOptions, bool isResetPasswordPage);
    }
}
