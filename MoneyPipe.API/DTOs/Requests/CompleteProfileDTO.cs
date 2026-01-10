using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs.Requests
{
    public class CompleteProfileDTO
    {
        [Required,Phone(ErrorMessage = "Provided phone number is not valid.")]
        public string PhoneNumber { get; set; } = null!;

        [Required,MinLength(1)]
        public string Street {get;set;} = null!;

        [Required,MinLength(1)]
        public string City {get; set;} = null!;

        [Required,MinLength(1)]
        public string State {get;set;} = null!;

        [Required,MinLength(1)]
        public string Country {get;set;} = null!;

        [Required,MinLength(1)]
        public string PostalCode {get;set;} = null!;

        [Required]
        public BaseDocumentDTO NIN {get;set;} = null!;
    }
}