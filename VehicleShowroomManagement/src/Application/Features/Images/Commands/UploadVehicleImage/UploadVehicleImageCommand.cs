using MediatR;
using Microsoft.AspNetCore.Http;

namespace VehicleShowroomManagement.Application.Features.Images.Commands.UploadVehicleImage
{
    /// <summary>
    /// Command for uploading vehicle images
    /// </summary>
    public record UploadVehicleImageCommand(string VehicleId, List<IFormFile> Images) : IRequest<List<VehicleImageResult>>;

    /// <summary>
    /// Result for uploaded vehicle image
    /// </summary>
    public class VehicleImageResult
    {
        public string ImageId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int FileSize { get; set; }
    }
}