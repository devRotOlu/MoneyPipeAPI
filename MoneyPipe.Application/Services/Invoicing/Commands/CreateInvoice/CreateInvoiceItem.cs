namespace MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice
{
    public record CreateInvoiceItem
    {
        public string Description {get;init;} = null!;
        public int? Quantity {get;init;}
        public decimal? UnitPrice {get;init;}
        public decimal? TotalPrice {get;init;}
    };
}