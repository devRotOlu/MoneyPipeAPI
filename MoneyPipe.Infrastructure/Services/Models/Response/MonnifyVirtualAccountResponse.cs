namespace MoneyPipe.Infrastructure.Services.Models.Response
{
    public sealed class MonnifyVirtualAccountResponse
    {
        public bool RequestSuccessful { get; set; }
        public ReservedAccountBody ResponseBody { get; set; } = default!;
        public class ReservedAccountBody
        {
            public string AccountReference { get; set; } = default!;
            public List<AccountDetail> Accounts { get; set; } = new();
        }

        public class AccountDetail
        {
            public string AccountNumber { get; set; } = default!;
            public string BankName { get; set; } = default!;
        }
    }
}