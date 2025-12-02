using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs
{
    public record CreateInvoiceDTO
    {
        public string Currency { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "Customer's email is required"),
        EmailAddress(ErrorMessage = "Invalid email format")]
        public string CustomerEmail { get; set; } = null!;
        [Required(ErrorMessage ="Customer name must be filled")]
        public string CustomerName { get; set; } = null!;
        public string? CustomerAddress { get; set; }
        [Required(ErrorMessage ="At least one invoice item must be inputed.")]
        public IEnumerable<CreateInvoiceItemsDTO> InvoiceItems { get; set; } = null!;
    }
}