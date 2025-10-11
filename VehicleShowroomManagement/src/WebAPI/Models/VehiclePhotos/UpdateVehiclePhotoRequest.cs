namespace VehicleShowroomManagement.WebAPI.Models.VehiclePhotos
{
    /// <summary>
    /// Request model for updating a vehicle photo
    /// </summary>
    public class UpdateVehiclePhotoRequest
    {
        public string? Url { get; set; }
        public int? DisplayOrder { get; set; }
        public string? Caption { get; set; }
    }
}

