using ErrorOr;

namespace MoneyPipe.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Address
        {
            public static Error InvalidCity(object providedValue) => Error.Validation(
                code: $"{providedValue}.Address.InvalidCity",
                description: $"The city is not valid. Please provide a valid city name."
            );

            public static Error InvalidStreet(object providedValue) => Error.Validation(
                code: $"{providedValue}.Address.InvalidStreet",
                description: $"The street is not valid. Street names must contain letters and numbers."
            );

            public static Error InvalidCountry(object providedValue) => Error.Validation(
                code: $"{providedValue}.Address.InvalidCountry",
                description: $"The country is not valid. Please provide a recognized country name."
            );

            public static Error InvalidState(object providedValue) => Error.Validation(
                code: $"{providedValue}.Address.InvalidState",
                description: $"The state is not valid. Please provide a valid state or region."
            );

            public static Error InvalidPostalCode(object providedValue) => Error.Validation(
                code: $"{providedValue}.Address.InvalidPostalCode",
                description: $"The postal code is not valid. Postal codes must follow the correct format."
            );
        }

    }
}