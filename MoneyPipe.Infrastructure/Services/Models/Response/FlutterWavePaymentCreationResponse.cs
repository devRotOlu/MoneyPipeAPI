namespace MoneyPipe.Infrastructure.Services.Models.Response
{
    class FlutterWavePaymentCreationResponse
    {
        public ResponseData Data {get;init;} = null!;
        public record ResponseData(string Link);
    }
}