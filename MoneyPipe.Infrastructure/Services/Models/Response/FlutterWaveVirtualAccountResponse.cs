namespace MoneyPipe.Infrastructure.Services.Models.Response
{
    public class FlutterWaveVirtualAccountResponse
    {
        public ResponseData Data {get;init;} = null!;
        public record ResponseData(string Id,string Account_Bank_Name,string Account_Number);
    }    
}