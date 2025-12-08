namespace MoneyPipe.Application.Services.Invoicing.Common
{
    public record InvoiceResult(Guid Id,string InvoiceNumber,decimal SubTotal,decimal TaxAmount,
    decimal TotalAmount,string Currency,string Status,DateTime? DueDate,DateTime? IssueDate,
    DateTime? PaidAt,string CustomerName,string CustomerEmail,string CustomerAddress,
    string Notes,string PaymentUrl,IEnumerable<InvoiceItemResult> InvoiceItems);
}