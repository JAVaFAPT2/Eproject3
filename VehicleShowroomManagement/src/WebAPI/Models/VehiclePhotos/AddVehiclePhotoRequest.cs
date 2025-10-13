namespace VehicleShowroomManagement.WebAPI.Models.VehiclePhotos
{
    /// <summary>
    /// Request model for adding a vehicle photo
    /// </summary>
    public class AddVehiclePhotoRequest
    {
        public string Url { get; set; } = string.Empty;
        public string? VehicleModelId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string? Caption { get; set; }
    }
}

