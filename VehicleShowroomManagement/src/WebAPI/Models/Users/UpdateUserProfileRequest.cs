namespace VehicleShowroomManagement.WebAPI.Models.Users
{
    /// <summary>
    /// Request model for updating user profile
    /// </summary>
    public class UpdateUserProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }
}