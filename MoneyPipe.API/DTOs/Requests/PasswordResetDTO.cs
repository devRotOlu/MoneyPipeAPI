using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs.Requests
{
    public record PasswordResetDTO
    {
        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; } 

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; init; } = null!;

        [Required, Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmedPassword { get; init; } = null!;
    }
}