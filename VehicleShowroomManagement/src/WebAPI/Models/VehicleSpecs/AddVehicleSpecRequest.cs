namespace VehicleShowroomManagement.WebAPI.Models.VehicleSpecs
{
    /// <summary>
    /// Request model for adding a vehicle specification
    /// </summary>
    public class AddVehicleSpecRequest
    {
        public string SpecName { get; set; } = string.Empty;
        public string SpecValue { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public string? GroupName { get; set; }
    }
}

