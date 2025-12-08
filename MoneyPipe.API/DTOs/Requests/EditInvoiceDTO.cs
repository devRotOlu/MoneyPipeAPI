using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs.Requests
{
    public record EditInvoiceDTO:CreateInvoiceDTO
    {
        [Required(ErrorMessage = "Invoice Id is required")]
        public Guid InvoiceId { get; init; } 

        [Required(ErrorMessage ="Invoice items are required")]
        public new IEnumerable<EditInvoiceItemDTO> InvoiceItems { get; init; } = null!;
    }
}