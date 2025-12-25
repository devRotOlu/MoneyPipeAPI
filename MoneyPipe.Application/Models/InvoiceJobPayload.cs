namespace MoneyPipe.Application.Models
{
    public record InvoiceJobPayload
    {
        public InvoiceJobPayload(string invoiceId)
        {
            InvoiceId = invoiceId;
        }
        public string InvoiceId {get;}

        public override string ToString()
        {
            return string.Format("{{\"invoiceid\": \"{0}\"}}", InvoiceId);
        }
    }
}