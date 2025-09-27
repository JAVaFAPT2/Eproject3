namespace VehicleShowroomManagement.WebAPI.Models.Auth
{
    /// <summary>
    /// Request model for login
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}