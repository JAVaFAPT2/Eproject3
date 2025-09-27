using Microsoft.AspNetCore.Http;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for Cloudinary image service operations
    /// </summary>
    public interface ICloudinaryService
    {
        /// <summary>
        /// Uploads an image to Cloudinary
        /// </summary>
        Task<CloudinaryUploadResult> UploadImageAsync(IFormFile file, string folder);

        /// <summary>
        /// Deletes an image from Cloudinary
        /// </summary>
        Task<bool> DeleteImageAsync(string publicId);

        /// <summary>
        /// Gets optimized image URL
        /// </summary>
        string GetOptimizedImageUrl(string publicId, int width = 0, int height = 0);
    }

    /// <summary>
    /// Result from Cloudinary upload operation
    /// </summary>
    public class CloudinaryUploadResult
    {
        public string PublicId { get; set; } = string.Empty;
        public string SecureUrl { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; } = string.Empty;
        public long Bytes { get; set; }
    }
}