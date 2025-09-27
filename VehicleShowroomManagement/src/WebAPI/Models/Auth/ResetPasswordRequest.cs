namespace VehicleShowroomManagement.WebAPI.Models.Auth
{
    /// <summary>
    /// Request model for reset password
    /// </summary>
    public class ResetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}