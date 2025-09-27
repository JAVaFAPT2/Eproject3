using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleImages.Queries.GetVehicleImageById
{
    public class VehicleImageDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public string PublicId { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static VehicleImageDto FromEntity(VehicleImage vehicleImage)
        {
            return new VehicleImageDto
            {
                Id = vehicleImage.Id,
                VehicleId = vehicleImage.VehicleId,
                ImageUrl = vehicleImage.ImageUrl,
                ImageType = vehicleImage.ImageType,
                FileName = vehicleImage.FileName,
                FileSize = vehicleImage.FileSize,
                PublicId = vehicleImage.PublicId,
                OriginalFileName = vehicleImage.OriginalFileName,
                ContentType = vehicleImage.ContentType,
                IsPrimary = vehicleImage.IsPrimary,
                UploadedAt = vehicleImage.UploadedAt,
                CreatedAt = vehicleImage.CreatedAt,
                UpdatedAt = vehicleImage.UpdatedAt
            };
        }
    }
}