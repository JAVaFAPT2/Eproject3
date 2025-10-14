namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for Vehicle Specs
    /// </summary>
    public class VehicleSpecDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string SpecName { get; set; } = string.Empty;
        public string SpecValue { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string? GroupName { get; set; }
    }
}
