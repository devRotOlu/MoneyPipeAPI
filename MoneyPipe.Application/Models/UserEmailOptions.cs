namespace MoneyPipe.Application.Models
{
    public record UserEmailOptions
    {
        public string ToEmail { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string? HtmlContent { get; set; } 

        public string Message {get;set;} = null!;

        public List<KeyValuePair<string, string>> PlaceHolders { get; set; } = null!;
    }
}
