namespace MoneyPipe.Infrastructure.Services.Models.Response
{
    public sealed class KorapayVirtualAccountResponse
    {
        public AccountData Data {get;set;} = null!;
        public class AccountData {
            public string Account_Number {get;set;} = null!;
            public string Bank_Name {get;set;} = null!;
            public string Unique_Id {get;set;} = null!;
        }
    }
}