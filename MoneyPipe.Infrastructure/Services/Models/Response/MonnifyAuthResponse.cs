namespace MoneyPipe.Infrastructure.Services.Models.Response
{
    public class MonnifyAuthResponse
    {
        public AuthBody ResponseBody { get; set; } = default!;
        public class AuthBody
        {
            public string AccessToken { get; set; } = default!;
        }
    }
}