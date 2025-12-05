namespace MoneyPipe.API.DTOs.Responses
{
    public record UserDetailsDTO
    {
        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string Id { get; init; } = null!;
    }
}
