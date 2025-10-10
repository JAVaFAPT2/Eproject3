namespace VehicleShowroomManagement.WebAPI.Models.Users
{
    /// <summary>
    /// Request model for updating user profile (unified schema)
    /// </summary>
    public class UpdateUserProfileRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
