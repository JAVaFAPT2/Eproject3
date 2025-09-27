using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleImages.Commands.CreateVehicleImage
{
    public class CreateVehicleImageCommand : IRequest<string>
    {
        public string VehicleId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageType { get; set; } = "Exterior";
        public string FileName { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public string? PublicId { get; set; }
        public string? OriginalFileName { get; set; }
        public string? ContentType { get; set; }
        public bool IsPrimary { get; set; } = false;
    }
}