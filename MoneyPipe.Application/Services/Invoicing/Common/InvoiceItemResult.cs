namespace MoneyPipe.Application.Services.Invoicing.Common
{
    public record InvoiceItemResult(string Id,string Description, int Quantity,
    decimal UnitPrice,decimal TotalPrice);
}