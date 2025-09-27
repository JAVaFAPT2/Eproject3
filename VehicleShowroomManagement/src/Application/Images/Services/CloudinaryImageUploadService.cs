using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace VehicleShowroomManagement.Application.Images.Services
{
    public class CloudinaryImageUploadService : IImageUploadService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageUploadService(IConfiguration configuration)
        {
            var cloudinaryConfig = configuration.GetSection("CloudinarySettings");
            var account = new Account(
                cloudinaryConfig["CloudName"],
                cloudinaryConfig["ApiKey"],
                cloudinaryConfig["ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string? folder = null, int? width = null, int? height = null, string? transformation = null)
        {
            if (imageStream == null || imageStream.Length == 0)
                throw new ArgumentException("Image stream cannot be null or empty.", nameof(imageStream));

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, imageStream),
                Folder = folder ?? "vehicle-showroom",
                Transformation = new Transformation()
                    .Width(width ?? 500)
                    .Height(height ?? 500)
                    .Crop("fill")
                    .Gravity("face")
            };

            if (!string.IsNullOrEmpty(transformation))
            {
                uploadParams.Transformation = new Transformation(transformation);
            }

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return uploadResult.PublicId;
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }

        public string GetOptimizedImageUrl(string publicId, int? width = null, int? height = null, string? transformation = null)
        {
            var transformationObj = new Transformation()
                .Width(width ?? 500)
                .Height(height ?? 500)
                .Crop("fill")
                .Gravity("face");
            
            var url = _cloudinary.Api.UrlImgUp.Transform(transformationObj).BuildUrl(publicId);

            return url;
        }
    }
}