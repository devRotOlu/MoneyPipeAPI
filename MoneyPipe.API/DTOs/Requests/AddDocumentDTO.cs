using System.ComponentModel.DataAnnotations;

namespace MoneyPipe.API.DTOs.Requests
{
    public class AddDocumentDTO:BaseDocumentDTO
    {
        [Required,MinLength(1)]
        public string Type {get; init;} = null!;

        [Required,MinLength(1)]
        public string Issuer {get; init;} = null!;
    }
}