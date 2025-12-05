using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs.Requests
{
    public record CreateInvoiceDTO
    {
        public string Currency { get; init; } = null!;

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; init; }
        public DateTime? DueDate { get; init; }

        [Required(ErrorMessage = "Customer's email is required"),
        EmailAddress(ErrorMessage = "Invalid email format")]
        public string CustomerEmail { get; init; } = null!;
        [Required(ErrorMessage ="Customer name must be filled")]
        public string CustomerName { get; init; } = null!;
        public string? CustomerAddress { get; init; }
        [Required(ErrorMessage ="At least one invoice item must be inputed.")]
        public IEnumerable<CreateInvoiceItemDTO> InvoiceItems { get; init; } = null!;
    }
}