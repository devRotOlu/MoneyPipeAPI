namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadPdfAsync(byte[] fileBytes, string fileName);
    }
}