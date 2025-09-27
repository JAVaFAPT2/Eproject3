using MediatR;
using Microsoft.AspNetCore.Http;

namespace VehicleShowroomManagement.Application.Images.Commands
{
    /// <summary>
    /// Command to upload vehicle image to Cloudinary
    /// </summary>
    public class UploadVehicleImageCommand : IRequest<string>
    {
        public IFormFile ImageFile { get; set; } = null!;
        public string VehicleId { get; set; } = string.Empty;
        public string? Folder { get; set; } = "vehicles";
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string? Transformation { get; set; }
    }
}
