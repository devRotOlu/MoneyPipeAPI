using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.Application.DTOs
{
    public record LoginDTO
    {
        [Required, DataType(DataType.EmailAddress, ErrorMessage = "Invalid email.")]
        public string Email { get; init; } = null!;

        [Required, DataType(DataType.Password), StringLength(15, ErrorMessage = "Your password is longer than required")]
        public string Password { get; init; } = null!;
    }
}
