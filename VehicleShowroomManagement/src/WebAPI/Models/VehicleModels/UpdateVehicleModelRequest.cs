namespace VehicleShowroomManagement.WebAPI.Models.VehicleModels
{
    public class UpdateVehicleModelRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ParentId { get; set; }
        public int Level { get; set; } = 1;
        public string? Slug { get; set; }
    }
}
