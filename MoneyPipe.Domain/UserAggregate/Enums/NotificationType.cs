namespace MoneyPipe.Domain.UserAggregate.Enums
{
    public enum NotificationType
    {
        General = 0,
        InvoiceCreated = 1,
        InvoicePaid = 2,
        PayoutProcessed = 3,
        PaymentReceived = 4,
        VirtualCardCreated = 5,
        VirtualCardDebited = 6,
        VirtualAccountCredited = 7,
        SupportTicketMessage = 8,
        SecurityAlert = 9,
    }
}