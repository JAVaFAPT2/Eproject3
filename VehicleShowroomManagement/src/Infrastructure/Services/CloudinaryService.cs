using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Common.Configuration;
using VehicleShowroomManagement.Application.Common.Exceptions;
using Polly;
using Microsoft.Extensions.Logging;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Implementation of Cloudinary image service with resilience policies
    /// </summary>
    public class CloudinaryService : BaseService, ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly AsyncPolicy _resiliencePolicy;

        public CloudinaryService(IOptions<CloudinarySettings> options, ILogger<CloudinaryService> logger, AsyncPolicy resiliencePolicy) 
            : base(logger)
        {
            var settings = options.Value;
            
            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret);

            _cloudinary = new Cloudinary(account);
            _resiliencePolicy = resiliencePolicy;
        }

        public async Task<CloudinaryUploadResult> UploadImageAsync(IFormFile file, string folder)
        {
            LogOperationStart(nameof(UploadImageAsync), new { fileName = file.FileName, folder });

            if (file.Length <= 0)
            {
                LogOperationError(nameof(UploadImageAsync), new ArgumentException("File is empty"));
                throw new CloudinaryException("File is empty");
            }

            try
            {
                var result = await _resiliencePolicy.ExecuteAsync(async () =>
                {
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
                        throw new CloudinaryException($"Image upload failed: {uploadResult.Error.Message}");

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
                });

                LogOperationComplete(nameof(UploadImageAsync), new { publicId = result.PublicId, url = result.SecureUrl });
                return result;
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(UploadImageAsync), ex, new { fileName = file.FileName, folder });
                throw new CloudinaryException($"Failed to upload image: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            LogOperationStart(nameof(DeleteImageAsync), new { publicId });

            try
            {
                var result = await _resiliencePolicy.ExecuteAsync(async () =>
                {
                    var deletionParams = new DeletionParams(publicId);
                    var deleteResult = await _cloudinary.DestroyAsync(deletionParams);
                    return deleteResult.Result == "ok";
                });

                LogOperationComplete(nameof(DeleteImageAsync), new { publicId, success = result });
                return result;
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(DeleteImageAsync), ex, new { publicId });
                throw new CloudinaryException($"Failed to delete image: {ex.Message}", ex);
            }
        }

        public string GetOptimizedImageUrl(string publicId, int width = 0, int height = 0)
        {
            LogOperationStart(nameof(GetOptimizedImageUrl), new { publicId, width, height });

            try
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

                var url = _cloudinary.Api.UrlImgUp.Transform(transformation).BuildUrl(publicId);
                
                LogOperationComplete(nameof(GetOptimizedImageUrl), new { publicId, url });
                return url;
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(GetOptimizedImageUrl), ex, new { publicId, width, height });
                throw new CloudinaryException($"Failed to generate optimized URL: {ex.Message}", ex);
            }
        }
    }
}