namespace MoneyPipe.Application.Interfaces.IServices
{
    public record UserEmailOptions
    {
        public List<string> ToEmails { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string Body { get; set; } = null!;

        public List<KeyValuePair<string, string>> PlaceHolders { get; set; } = null!;
    }
}
