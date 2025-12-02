namespace MoneyPipe.Application.ReadModels
{
    public record NotificationDTO(Guid Id,string Title, string Message,string MetadataJson,
    string Type,bool IsRead,DateTime ReadAt,DateTime CreatedAt);
}