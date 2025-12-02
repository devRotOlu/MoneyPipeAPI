using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Interfaces;
using MoneyPipe.Domain.Models;

namespace MoneyPipe.Domain.Entities
{
    public class Invoice
    {
        private const decimal TaxRate = 0.10m;
        public Guid Id { get; private set; }
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

        private Invoice(){}

        public static ErrorOr<Invoice> Create(InvoiceRequest request)
        {
            var invoice = new Invoice();

            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(request.CustomerEmail)) errors.Add(Errors.Invoice.InvalidCustomerEmail);

            ErrorOr<Success> result = AddAndValidateInvoiceItems(request, invoice);

            if (result.IsError) errors.AddRange(result.Errors);

            if (errors.Count > 0)
                return errors;

            MapRequestToInvoice(invoice, request);

            return invoice;
        }

        public static ErrorOr<Invoice> Edit(EditInvoiceRequest request)
        {

            var invoice = new Invoice();
            
            var errors = new List<Error>();

            if (Guid.Empty == request.Id) errors.Add(Errors.Invoice.IdRequired);

            if (string.IsNullOrWhiteSpace(request.CustomerEmail)) errors.Add(Errors.Invoice.InvalidCustomerEmail);

            ErrorOr<Success> result = AddAndValidateInvoiceItems(request, invoice);

            if (result.IsError) errors.AddRange(result.Errors);

            if (errors.Count > 0) return errors;

            invoice.Id = request.Id;
            MapRequestToInvoice(invoice, request);

            return invoice;
        }

        private static void MapRequestToInvoice(Invoice invoice,InvoiceRequest request)
        {
            invoice.Currency = request.Currency;
            invoice.DueDate = request.DueDate;
            invoice.CustomerAddress = request.CustomerAddress;
            invoice.CustomerName = request.CustomerName;
            invoice.CustomerEmail = request.CustomerEmail;
            invoice.Notes = request.Notes;
        }

        public void MarkAsPaid()
        {
            Status = "Paid";
            PaidAt = DateTime.UtcNow;
        }

        public void MarkCancelled() => Status = "Cancelled";

        /// <summary>
        /// Infrastructure-only method for rehydrating invoice items from persistence.
        /// Do not use in business logic.
        /// </summary>
        public void AddInvoiceItem(InvoiceItem invoiceItem) => _invoiceItems.Add(invoiceItem);

         public ErrorOr<Success> AddInvoiceItem(InvoiceItemRequest request)
        {
            var itemResult = InvoiceItem.Create(request);
            if (itemResult.IsError)
                return itemResult.Errors;
            _invoiceItems.Add(itemResult.Value);
            RecalculateTotals();
            return Result.Success;
        }

        public void SetUserId(Guid userId) => UserId = userId;

        public void RecalculateTotals()
        {
            SubTotal = _invoiceItems.Sum(item => item.TotalPrice == null? 0 : item.TotalPrice );
            TaxAmount = SubTotal * TaxRate;
            TotalAmount = SubTotal + TaxAmount;
        }

        public void SetInvoiceNumber(int serialNumber) => InvoiceNumber = $"INV-{serialNumber:D6}";

        private static ErrorOr<Success> AddAndValidateInvoiceItems(IInvoiceRequest request,Invoice invoice)
        {
            var errors = new List<Error>();

            if (request.InvoiceItems == null || !request.InvoiceItems.Any())
            {
                errors.Add(Errors.Invoice.InvoiceItemError);
                return errors;
            }

            foreach (var itemRequest in request.InvoiceItems)
            {
                ErrorOr<Success> itemResult = invoice.AddInvoiceItem(itemRequest);

                if (itemResult.IsError)
                {
                    errors.AddRange(itemResult.Errors);
                    return errors;
                }
            }

            if (errors.Count > 0) return errors;

            return Result.Success;
        }
    }
}
