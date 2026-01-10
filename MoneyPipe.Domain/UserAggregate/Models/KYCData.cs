namespace MoneyPipe.Domain.UserAggregate.Models
{
    public record KYCData(string PhoneNumber, string City, 
    string Street, string PostalCode,string Country,
    string State);
}