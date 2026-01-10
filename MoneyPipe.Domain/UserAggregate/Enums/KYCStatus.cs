namespace MoneyPipe.Domain.UserAggregate.Enums
{
    public enum KYCStatus
    {
        NotStarted,
        InProgress,
        Submitted,
        PartiallyVerified,
        Verified,
        Rejected,
        Suspended
    }
}