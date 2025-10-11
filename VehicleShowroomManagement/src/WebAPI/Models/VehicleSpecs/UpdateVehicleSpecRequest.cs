namespace VehicleShowroomManagement.WebAPI.Models.VehicleSpecs
{
    /// <summary>
    /// Request model for updating a vehicle specification
    /// </summary>
    public class UpdateVehicleSpecRequest
    {
        public string? SpecValue { get; set; }
        public int? DisplayOrder { get; set; }
        public string? GroupName { get; set; }
    }
}

