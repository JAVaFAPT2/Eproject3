namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for Vehicle Photos
    /// </summary>
    public class VehiclePhotoDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string? VehicleModelId { get; set; }
        public string Url { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string? Caption { get; set; }
    }
}
