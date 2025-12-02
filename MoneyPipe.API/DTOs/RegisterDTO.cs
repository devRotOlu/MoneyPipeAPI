using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs
{
    public record RegisterDto : LoginDTO
    {
        [Required]
        public string FirstName { get; init; } = null!;

        [Required]
        public string LastName { get; init; } = null!;
    }
}
