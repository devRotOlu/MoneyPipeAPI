using System.ComponentModel.DataAnnotations;
using MoneyPipe.Domain.InvoiceAggregate.Enums;

namespace MoneyPipe.API.DTOs.Requests
{
    public record SendInvoiceDTO
    {
        [Required]
        public Guid InvoiceId {get;set;}
        [Required]
        public Guid WalletId {get;set;}
        [Required]
        public PaymentMethod PaymentMethod {get;set;} 
    }
}