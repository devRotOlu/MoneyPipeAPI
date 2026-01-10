using System.ComponentModel.DataAnnotations;
using MoneyPipe.Domain.UserAggregate.Enums;

namespace MoneyPipe.API.DTOs.Requests
{
    public class BaseDocumentDTO
    {
        [Required]
        public IdentityCategory Category {get; set;}

        [Required,MinLength(1)]
        public string Value {get; set;} = null!;
    }
}