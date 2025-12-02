using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs
{
    public record LoginDTO
    {
        [Required, DataType(DataType.EmailAddress, ErrorMessage = "Invalid email.")]
        public string Email { get; init; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; init; } = null!;
    }
}
