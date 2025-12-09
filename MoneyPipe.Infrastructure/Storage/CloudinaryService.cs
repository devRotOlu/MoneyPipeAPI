using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using MoneyPipe.Application.Interfaces.IServices;

namespace MoneyPipe.Infrastructure.Storage
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var settings = configuration.GetSection("Cloudinary");

            var account = new Account(
                settings["CloudName"],
                settings["ApiKey"],
                settings["ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadPdfAsync(byte[] fileBytes, string fileName)
        {
            using var stream = new MemoryStream(fileBytes);

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, stream),
                PublicId = fileName,        
                Overwrite = true,
            };

            RawUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl.ToString();
        }
    }
}