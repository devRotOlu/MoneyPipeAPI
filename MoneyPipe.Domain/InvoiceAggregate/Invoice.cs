using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.InvoiceAggregate.Entities;
using MoneyPipe.Domain.InvoiceAggregate.Enums;
using MoneyPipe.Domain.InvoiceAggregate.Events;
using MoneyPipe.Domain.InvoiceAggregate.Models;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.InvoiceAggregate
{
    public class Invoice:AggregateRoot<InvoiceId>
    {
        private const decimal TaxRate = 0.10m;
        private readonly List<InvoiceItem> _invoiceItems = [];
        public UserId UserId { get; private set; }
        public string InvoiceNumber { get; private set;} = null!;
        public decimal? SubTotal { get; private set; }
        public decimal? TaxAmount { get; private set; }
        public decimal? TotalAmount { get; private set; }
        public string? Currency { get; private set;} 
        public string Status { get; private set;} = InvoiceStatus.Draft.ToString();
        public DateTime? DueDate { get; private set;}
        public DateTime? IssueDate { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public DateTime CreatedAt {get;private set;}
        public DateTime UpdatedAt {get;private set;}
        public string CustomerName { get; private set; } = null!;
        public string CustomerEmail { get; private set; } = null!;
        public string? CustomerAddress { get; private set; } 
        public string? Notes { get; private set; }
        public string? PaymentUrl { get; private set; }
        public IReadOnlyCollection<InvoiceItem> InvoiceItems => _invoiceItems.AsReadOnly(); 

        private Invoice(){}

        private Invoice(InvoiceId id):base(id)
        {
            
        }

        public static ErrorOr<Invoice> Create(InvoiceData data)
        {
            var invoice = new Invoice(InvoiceId.CreateUnique(Guid.NewGuid()))
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(data.CustomerEmail)) errors.Add(Errors.Invoice.InvalidCustomerEmail);

             if (data.InvoiceItems == null || !data.InvoiceItems.Any())
            {
                errors.Add(Errors.Invoice.InvoiceItemError);
                return errors;
            }

            foreach (var itemdata in data.InvoiceItems)
            {
                ErrorOr<InvoiceItem> itemResult = InvoiceItem.Create(itemdata,invoice.Id);
                ErrorOr<Success> result = invoice.AddInvoiceItem(itemResult);

                if (result.IsError)
                {
                    errors.AddRange(itemResult.Errors);
                    return errors;
                }
            }

            if (errors.Count > 0)
                return errors;

            MapDataToInvoice(invoice, data);

            return invoice;
        }

        public static ErrorOr<Invoice> Edit(EditInvoiceData data)
        { 
            var errors = new List<Error>();

            if (Guid.Empty == data.Id) errors.Add(Errors.Invoice.IdRequired);

            if (string.IsNullOrWhiteSpace(data.CustomerEmail)) errors.Add(Errors.Invoice.InvalidCustomerEmail);

            var invoice = new Invoice(InvoiceId.CreateUnique(data.Id))
            {
                UpdatedAt = DateTime.UtcNow
            };

            if (data.InvoiceItems == null || !data.InvoiceItems.Any())
            {
                errors.Add(Errors.Invoice.InvoiceItemError);
                return errors;
            }

            foreach (var item in data.InvoiceItems)
            {
                // determine if it's newly added item or just edited based on its id
                ErrorOr<InvoiceItem> itemResult;
                if (item.Id is null) itemResult = InvoiceItem.Create(item,invoice.Id);
                else itemResult = InvoiceItem.Edit(item,invoice.Id);
                ErrorOr<Success> result = invoice.AddInvoiceItem(itemResult);
                if (result.IsError)
                {
                    errors.AddRange(itemResult.Errors);
                    return errors;
                }
            }

            if (errors.Count > 0) return errors;

            MapDataToInvoice(invoice, data);

            return invoice;
        }

        private static void MapDataToInvoice(Invoice invoice,InvoiceData data)
        {
            invoice.Currency = data.Currency??"NGN";
            invoice.DueDate = data.DueDate;
            invoice.CustomerAddress = data.CustomerAddress;
            invoice.CustomerName = data.CustomerName;
            invoice.CustomerEmail = data.CustomerEmail;
            invoice.Notes = data.Notes;
        }

        public void MarkAsPaid()
        {
            Status = InvoiceStatus.Paid.ToString();
            PaidAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkCancelled()
        {
            Status = InvoiceStatus.Cancelled.ToString();
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkSent()
        {
            Status = InvoiceStatus.Sent.ToString();
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new InvoiceCreatedEvent(this,UserId.Value,CustomerEmail));
        }

        /// <summary>
        /// Infrastructure-only method for rehydrating invoice items from persistence.
        /// Do not use in business logic.
        /// </summary>
        public void AddInvoiceItems(IEnumerable<InvoiceItem> invoiceItems) => _invoiceItems.AddRange(invoiceItems);

        public ErrorOr<Success> AddInvoiceItem(ErrorOr<InvoiceItem> itemResult)
        {
            if (itemResult.IsError)
                return itemResult.Errors;
            _invoiceItems.Add(itemResult.Value);
            RecalculateTotals();
            return Result.Success;
        }

        public void SetUserId(Guid userId) => UserId = UserId.CreateUnique(userId);

        private void RecalculateTotals()
        {
            SubTotal = _invoiceItems.Sum(item => item.TotalPrice == null? 0 : item.TotalPrice );
            TaxAmount = SubTotal * TaxRate;
            TotalAmount = SubTotal + TaxAmount;
        }

        public void SetInvoiceNumber(int serialNumber) => InvoiceNumber = $"INV-{serialNumber:D6}";
    }
}