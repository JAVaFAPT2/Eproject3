namespace VehicleShowroomManagement.WebAPI.Models.Profile
{
    /// <summary>
    /// Request model for updating profile
    /// </summary>
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }
}