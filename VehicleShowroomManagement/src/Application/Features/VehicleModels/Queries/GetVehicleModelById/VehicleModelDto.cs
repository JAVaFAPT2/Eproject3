namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelById
{
    /// <summary>
    /// Data Transfer Object for Vehicle Model
    /// </summary>
    public class VehicleModelDto
    {
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
