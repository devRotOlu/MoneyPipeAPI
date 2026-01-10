using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.UserAggregate.ValueObjects
{
    public class Address:ValueObject
    {
        private Address()
        {
            
        }

        public string Street { get;  private set; } = null!;
        public string City { get;  private set; } = null!;
        public string State { get; private set; } = null!;
        public string Country { get;  private set; } = null!;
        public string PostalCode { get;  private set; } = null!;

        public static ErrorOr<Address> Create(string street,string city, string state, 
        string country,string postalCode,Type objectType)
        {
            List<Error> errors = [];
            var objectName = nameof(objectType);

            if (string.IsNullOrWhiteSpace(street))
               errors.Add(Errors.Address.InvalidStreet(objectName));
            if (string.IsNullOrWhiteSpace(city))
                errors.Add(Errors.Address.InvalidCity(objectName));
            if (string.IsNullOrWhiteSpace(state)) 
                errors.Add(Errors.Address.InvalidState(objectName));
            if (string.IsNullOrWhiteSpace(country)) 
                errors.Add(Errors.Address.InvalidCountry(objectName));
            if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length < 4) 
                errors.Add(Errors.Address.InvalidPostalCode(objectName));;
            
            return errors.Count > 0? errors: new Address()
            {
                Street = street,
                City = city,
                State = state,
                Country = country,
                PostalCode = postalCode
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return PostalCode;
        }
    }
}