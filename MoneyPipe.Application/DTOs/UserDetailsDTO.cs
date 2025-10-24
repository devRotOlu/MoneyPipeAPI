namespace MoneyPipe.Application.DTOs
{
    public record UserDetailsDTO
    {
        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string Id { get; init; } = null!;
    }
}
