using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs
{
    public record EditInvoiceDTO:CreateInvoiceDTO
    {
        [Required(ErrorMessage = "Invoice Id is required")]
        public string Id { get; private set; } = null!;

        [Required(ErrorMessage ="Invoice items are required")]
        public new IEnumerable<EditInvoiceItemDTO> InvoiceItems { get; set; } = null!;
    }
}