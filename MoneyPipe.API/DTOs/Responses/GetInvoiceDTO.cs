namespace MoneyPipe.API.DTOs.Responses
{
    public record GetInvoiceDTO(IEnumerable<GetInvoiceItemDTO> InvoiceItems):GetInvoiceBase;
}