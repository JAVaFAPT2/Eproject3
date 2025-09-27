namespace VehicleShowroomManagement.Application.Images.Services
{
    /// <summary>
    /// Interface for image upload services
    /// </summary>
    public interface IImageUploadService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName, string? folder = null, int? width = null, int? height = null, string? transformation = null);
        Task<bool> DeleteImageAsync(string publicId);
        string GetOptimizedImageUrl(string publicId, int? width = null, int? height = null, string? transformation = null);
    }
}
