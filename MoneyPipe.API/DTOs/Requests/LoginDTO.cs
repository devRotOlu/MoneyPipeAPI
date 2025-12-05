using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs.Requests
{
    public record LoginDTO
    {
        [Required, DataType(DataType.EmailAddress, ErrorMessage = "Invalid email.")]
        public string Email { get; init; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; init; } = null!;
    }
}
