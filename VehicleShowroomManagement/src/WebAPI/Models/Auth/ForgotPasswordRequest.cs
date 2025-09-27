namespace VehicleShowroomManagement.WebAPI.Models.Auth
{
    /// <summary>
    /// Request model for forgot password
    /// </summary>
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}