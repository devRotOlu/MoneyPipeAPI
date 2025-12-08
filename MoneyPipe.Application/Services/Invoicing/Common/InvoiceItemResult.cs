namespace MoneyPipe.Application.Services.Invoicing.Common
{
    public record InvoiceItemResult(Guid Id,string Description, int Quantity,
    decimal UnitPrice,decimal TotalPrice);
}