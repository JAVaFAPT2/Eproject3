using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Implementation of Cloudinary image service
    /// </summary>
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<CloudinaryUploadResult> UploadImageAsync(IFormFile file, string folder)
        {
            if (file.Length <= 0)
                throw new ArgumentException("File is empty");

            using var stream = file.OpenReadStream();
            
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception($"Image upload failed: {uploadResult.Error.Message}");

            return new CloudinaryUploadResult
            {
                PublicId = uploadResult.PublicId,
                SecureUrl = uploadResult.SecureUrl.ToString(),
                Url = uploadResult.Url.ToString(),
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Format = uploadResult.Format,
                Bytes = uploadResult.Bytes
            };
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);

            return result.Result == "ok";
        }

        public string GetOptimizedImageUrl(string publicId, int width = 0, int height = 0)
        {
            var transformation = new Transformation();

            if (width > 0 && height > 0)
            {
                transformation.Width(width).Height(height).Crop("fill");
            }
            else if (width > 0)
            {
                transformation.Width(width);
            }
            else if (height > 0)
            {
                transformation.Height(height);
            }

            transformation.Quality("auto").FetchFormat("auto");

            return _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
        }
    }
}