using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.InvoiceAggregate.Entities;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Domain.InvoiceAggregate
{
    public class Invoice:AggregateRoot<InvoiceId>
    {
        private const decimal TaxRate = 0.10m;
        private readonly List<InvoiceItem> _invoiceItems = [];
        public Guid UserId { get; private set; }
        public string InvoiceNumber { get; private set;} = null!;
        public decimal? SubTotal { get; private set; }
        public decimal? TaxAmount { get; private set; }
        public decimal? TotalAmount { get; private set; }
        public string Currency { get; private set;} = string.Empty;
        public string Status { get; private set;} = "Draft";
        public DateTime? DueDate { get; private set;}
        public DateTime IssueDate { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public string CustomerName { get; private set; } = null!;
        public string CustomerEmail { get; private set; } = null!;
        public string? CustomerAddress { get; private set; } 
        public string? Notes { get; private set; }
        public string? PaymentUrl { get; set; }
        public IReadOnlyCollection<InvoiceItem> InvoiceItems => _invoiceItems.AsReadOnly();    
    }
}